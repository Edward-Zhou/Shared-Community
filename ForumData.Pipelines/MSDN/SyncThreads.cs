using ForumData.Pipelines.Models;
using Kivi.Platform.Core.SDK;
using LemonCore.Core;
using LemonCore.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumData.Pipelines.MSDN
{
    public class SyncThreads : ICommand
    {
        private readonly string _localStageConnectionString;
        private HttpDonwloadAgent _agent;

        public SyncThreads(string localStageConnectionString)
        {
            _localStageConnectionString = localStageConnectionString;
            _agent = new HttpDonwloadAgent();
        }

        public bool Run(string arguments)
        {
            string[] tokens = arguments.Split(';');
            string startTime = tokens[0];
            string endTime = tokens[1];

            string where = string.Format("CreatedOn between '{0}' and '{1}'", startTime, endTime);
            var threads = new SqlDataReader<MsdnQuestionIndexEntity>(_localStageConnectionString, "msdn_question_index", "CreatedOn", where);
            var writter = new SqlDataWriter<MsdnQuestionIndexEntity>(_localStageConnectionString, "msdn_question_index", WriteMode.Update);

            Pipeline pipeline = new Pipeline { BoundedCapacity = 20 };
            pipeline.DataSource(threads)
                    .Transform(thread => UserProfileParser.Parse(thread))
                    .Output(writter);
            return PipelineUtil.BuildAndRun(pipeline);

        }
    }
}
