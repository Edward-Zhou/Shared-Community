using ForumData.Pipelines.Models;
using Kivi.Platform.Core.SDK;
using LemonCore.Core;
using LemonCore.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumData.Pipelines
{
    public class DownloadMSDNQuestions : ICommand
    {
        private readonly string _localStageConnectionString;
        private const string URL_FORMAT = "https://social.msdn.microsoft.com/Forums/office/en-US/home?forum={0}&filter={1}&sort={2}&brandIgnore=true&page={3}";
        private HttpDonwloadAgent _agent;

        public DownloadMSDNQuestions(string localStageConnectionString)
        {
            _localStageConnectionString = localStageConnectionString;
            _agent = new HttpDonwloadAgent();
        }

        public bool Run(string arguments)
        {
            string[] tokens = arguments.Split(',');
            string forumId = tokens[0];
            string filter = tokens[1];
            string sort = tokens[2];
            int pageNum = int.Parse(tokens[3]);

            var pipeline = new Pipeline { BoundedCapacity = 20 };
            var writter = new SqlDataWriter<MsdnQuestionIndexEntity>(_localStageConnectionString, "msdn_question_index", WriteMode.Upsert);
            pipeline.DataSource(new DatasourceWrapper<string>(GenerateRequests(forumId, filter, sort, pageNum)))
                     .Transform(url => _agent.GetString(url))
                     .TransformMany(html => MsdnIndexPageParser.Parse(html,DateTime.Now))   
                     .Output(writter);

            return PipelineUtil.BuildAndRun(pipeline);
        }
        private IEnumerable<string> GenerateRequests(string forumId, string filter, string sort, int pageNum)
        {
            var requests = new List<string>();
            for (int i = 1; i <= pageNum; i++)
            {
                requests.Add(string.Format(URL_FORMAT, forumId, filter, sort, i));
            }
            return requests;
        }

    }
}
