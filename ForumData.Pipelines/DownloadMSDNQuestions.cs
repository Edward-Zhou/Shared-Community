using ForumData.Pipelines.Models;
using ForumData.Pipelines.MSDN;
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
            string[] tokens = arguments.Split(';');
            string forumId = tokens[0];
            string filter = tokens[1];
            string sort = tokens[2];
            int pageNum = int.Parse(tokens[3]);

            var month = DateTime.Now.Day <= 5 ? DateTime.Now.Month - 1 : DateTime.Now.Month;

            var pipeline = new Pipeline { BoundedCapacity = 20 };
            var writter = new SqlDataWriter<MsdnQuestionIndexEntity>(_localStageConnectionString, "msdn_question_index", WriteMode.Upsert);
            pipeline.DataSource(new DatasourceWrapper<string>(GenerateRequests(forumId, filter, sort, pageNum)))
                     .Transform(url => _agent.GetString(url))
                     .TransformMany(html => MsdnIndexPageParser.Parse(html, DateTime.Now, new DateTime(DateTime.Now.Year, month, 1)))
                     .Transform(thread => UserProfileParser.Parse(thread))
                     .Output(writter);

            return PipelineUtil.BuildAndRun(pipeline);

        }
        private IEnumerable<string> GenerateRequests(string forumId, string filter, string sort, int pageNum = 20)
        {
            var requests = new List<string>();
            for (int i = pageNum; i >= 1; i--)
            {
                var request = string.Format(URL_FORMAT, forumId, filter, sort, i);
                requests.Add(request);
            }
            return requests;
        }

        //private IEnumerable<string> GenerateRequests(string forumId, string filter, string sort, int pageNum = 23)
        //{
        //    var requests = new List<string>();
        //    for (int i = pageNum; i >= 1; i--)
        //    {
        //        var request = string.Format(URL_FORMAT, forumId, filter, sort, i);
        //        string html = _agent.GetString(request);
        //        List<MsdnQuestionIndexEntity> threads = MsdnIndexPageParser.Parse(html, DateTime.Now).ToList();
        //        MsdnQuestionIndexEntity thread = threads.OrderByDescending(t => t.CreatedOn).FirstOrDefault();
        //        var month = DateTime.Now.Day <= 5 ? DateTime.Now.Month - 1 : DateTime.Now.Month;
        //        if (thread.CreatedOn <= new DateTime(DateTime.Now.Year, month, 1))
        //        {
        //            continue;
        //        }
        //        requests.Add(request);
        //    }
        //    return requests;
        //}


    }
}
