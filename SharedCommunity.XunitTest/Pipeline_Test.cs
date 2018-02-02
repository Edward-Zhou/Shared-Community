using ForumData.Pipelines;
using LemonCore.Core;
using LemonCore.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SharedCommunity.XunitTest
{
    public class Pipeline_Test
    {
        private readonly ITestOutputHelper output;
        public Pipeline_Test(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Test1()
        {
            Pipeline pipeline = new Pipeline { BoundedCapacity = 20 };
            var writter = new JsonConsoleWriter<string>();
            pipeline.DataSource(new DatasourceWrapper<string>(new List<string>() { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8" }))
                    .Transform(task => DoTask(task),5)
                    .Output(writter);
            PipelineUtil.BuildAndRun(pipeline);
        }
        string DoTask(string task)
        {
            //output.WriteLine(string.Format("Start processId {0}, Time : {1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss,fff")));
            //Thread.Sleep(1000);
            //output.WriteLine(string.Format("End processId {0}, Time : {1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss,fff")));
            //return "Completed";
            output.WriteLine(string.Format("Start processId {0}, Time : {1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss,fff")));
            Thread.Sleep(2000);
            var result = DoTaskAsync(task);
            output.WriteLine(string.Format("End processId {0}, Time : {1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss,fff")));

            return result;
        }
        string DoTaskAsync(string task)
        {
            var t = Task.Factory.StartNew(() => {
                //output.WriteLine(string.Format("Start processId {0}, Time : {1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss,fff")));
                Thread.Sleep(1000);
                //output.WriteLine(string.Format("End processId {0}, Time : {1}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss,fff")));
                return "Completed";
            });
            string ss = A().Result;
           return ss;
        }
        async Task<string> A()
        {
            Thread.Sleep(1000);
            return "r";
        }
        async Task<string> B()
        {
            Thread.Sleep(1000);
            return "h";
        }
    }
    internal class PipelineUtil
    {
        public static bool BuildAndRun(Pipeline pipeline)
        {
            var exe = pipeline.Build();
            var task = exe.RunAsync(null);
            task.Wait();
            return task.Result;
        }
    }
}
