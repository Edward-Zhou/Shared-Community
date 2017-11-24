using ForumData.Pipelines.Models;
using Kivi.Platform.Core.SDK;
using Lemon.Data.IO;
//using Lemon.Data.Core;
using LemonCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForumData.Pipelines
{
    public class DownloadMSDNQuestions : ICommand
    {
        private readonly string _localStageConnectionString;
        private HttpDonwloadAgent _agent;
        private class MSDNQuestionsDownloadRequest
        {
            public string Url { get; set; }
            public int ForumId { get; set; }
        }
        public DownloadMSDNQuestions(string localStageConnectionString)
        {
            _localStageConnectionString = localStageConnectionString;
            _agent = new HttpDonwloadAgent();
        }

        public bool Run(string arguments)
        {
            HttpDonwloadAgent donwloadAgent = new HttpDonwloadAgent();
            var pipeline = new Pipeline { BoundedCapacity = 20 };
            var writter = new SqlDataWriter<MsdnQuestionIndexEntity>(_localStageConnectionString, "msdn_question_index", WriteMode.Upsert);
            pipeline.DataSource(new DatasourceWrapper<string>(new string[] { "https://social.msdn.microsoft.com/Forums/office/en-US/home?forum=exceldev", /*"https://forums.asp.net"*/ }))
                     .Transform(url => donwloadAgent.GetString(url))
                     .TransformMany(html => MsdnIndexPageParser.Parse(html,DateTime.Now))   
                     .Transform(entity =>
                     {
                         //writter.Write(entity);
                         return entity;
                     })
                     .Output(new JsonConsoleWriter<MsdnQuestionIndexEntity>());

            return PipelineUtil.BuildAndRun(pipeline);
        }
    }
}
