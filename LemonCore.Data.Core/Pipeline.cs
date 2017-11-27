using LemonCore.Core.Interfaces;
using LemonCore.Core.Models;
using LemonCore.Core.Services;
using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LemonCore.Core
{
    public class Pipeline
    {
        private Node _root;
        public int BoundedCapacity { get; set; }
        protected readonly Guid _id;
        protected readonly ILog _logger;

        public Pipeline()
        {
            _id = Guid.NewGuid();
            //_logger = LogService.Default.GetLog("Pipeline");
        }
        public TransformActionChain<TSource> DataSource<TSource>(IDataReader<TSource> reader)
        {
            this._root = new DataSourceNode<TSource>
            {
                Reader = reader
            };
            return new TransformActionChain<TSource>(this._root);
        }

        public IExecute Build()
        {
            if (_root.NodeType != NodeType.SourceNode)
            {
                throw new Exception("the root node must be source node");
            }

            var reader = _root.GetType().GetProperty("Reader").GetValue(_root) as IDataReader;
            var sourceNode = _root as ISource;
            var bufferBlock = BlockBuilder.CreateBufferBlock(sourceNode.SourceType,
                new DataflowBlockOptions { BoundedCapacity = BoundedCapacity });
            var messageType = typeof(MessageWrapper<>).MakeGenericType(sourceNode.SourceType);

            var tasks = new List<Task>();

            var target = BuildTargetBlock(sourceNode.Next, tasks);
            bufferBlock.LinkTo(
                target,
                new DataflowLinkOptions
                {
                    PropagateCompletion = true
                });

            return new Execution(async (IDictionary<string, object> parameters) => {
                while (reader.Next())
                {
                    try
                    {
                        await bufferBlock.SendAsync(
                            Activator.CreateInstance(
                                messageType,
                                new object[] {
                                    reader.Read(),
                                    _id
                                }));
                    }
                    catch (Exception ex)
                    {
                        //Logger.Error("reader fail", ex);
                    }
                }

                // dispose the reader and signal the read completion
                reader.Dispose();
                bufferBlock.Complete();

                // use a task to wait all blocks completed
                return await Task.Run(() =>
                {
                    Task.WaitAll(tasks.ToArray());
                    return true;
                });
            });
        }

        private object BuildTargetBlock(Node node, IList<Task> tasks)
        {
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            var executionOptions = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = BoundedCapacity
            };

            if (node.NodeType == NodeType.ActionNode)
            {
                var target = node as ITarget;
                var nodeClass = typeof(ActionNode<>).MakeGenericType(target.TargetType);
                var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(target.TargetType);
                var writeFunc = nodeClass.GetProperty("Write").GetValue(node);
                var actionBlock = BlockBuilder.CreateActionBlock(target.TargetType, writeFunc, executionOptions);
                tasks.Add(actionBlock.Completion);

                return actionBlock;
            }
            else if (node.NodeType == NodeType.TransformNode)
            {
                var source = node as ISource;
                var target = node as ITarget;
                var nodeClass = typeof(TransformNode<,>).MakeGenericType(source.SourceType, target.TargetType);
                var transformFunc = nodeClass.GetProperty("Block").GetValue(node);
                var options = new ExecutionDataflowBlockOptions
                {
                    BoundedCapacity = executionOptions.BoundedCapacity
                };

                if (node.MaxDegreeOfParallelism.HasValue)
                {
                    options.MaxDegreeOfParallelism = node.MaxDegreeOfParallelism.Value;
                }

                var transformBlock = BlockBuilder.CreateTransformBlock(source.SourceType, target.TargetType, transformFunc, options);

                var targetBlock = BuildTargetBlock(source.Next, tasks);

                transformBlock.LinkTo(targetBlock, linkOptions);

                return transformBlock;
            }
            else if (node.NodeType == NodeType.TransformManyNode)
            {
                var source = node as ISource;
                var target = node as ITarget;
                var nodeClass = typeof(TransformManyNode<,>).MakeGenericType(source.SourceType, target.TargetType);
                var transformManyFunc = nodeClass.GetProperty("Block").GetValue(node);

                var options = new ExecutionDataflowBlockOptions
                {
                    BoundedCapacity = executionOptions.BoundedCapacity
                };

                if (node.MaxDegreeOfParallelism.HasValue)
                {
                    options.MaxDegreeOfParallelism = node.MaxDegreeOfParallelism.Value;
                }

                var transformManyBlock = BlockBuilder.CreateTransformManyBlock(source.SourceType, target.TargetType, transformManyFunc, options);
                var targetBlock = BuildTargetBlock(source.Next, tasks);
                transformManyBlock.LinkTo(targetBlock, linkOptions);

                return transformManyBlock;
            }
            else
            {
                //Logger.Error("the node type does not support building target block");
                throw new Exception("the node type does not support building target block");
            }
        }


        public TransformActionChain<TSource> Read<TSource>(IDataReader<TSource> reader)
        {
            _root = new DataSourceNode<TSource>
            {
                Reader = reader
            };
            return new TransformActionChain<TSource>(_root);
        }
    }
}
