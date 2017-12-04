using ForumData.Pipelines.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace ForumData.Pipelines
{
    public class MsdnIndexPageParser
    {
        public static IEnumerable<MsdnQuestionIndexEntity> Parse(string html, DateTime referTimestamp, DateTime startTimestamp)
        {
            var list = new List<MsdnQuestionIndexEntity>();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var nodes = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"threadList\"]/li//div[@class='detailscontainer']");
            if(nodes == null)
            {
                return list;
            }
            var numberRegex = new Regex(@"\d+");
            foreach (var node in nodes)
            {
                var h3link = node.SelectSingleNode("h3/a");
                var title = h3link.InnerText;
                var uri = h3link.Attributes["href"].Value;
                var id = h3link.Attributes["data-threadid"].Value;
                var metricsNode = node.SelectSingleNode("div[contains(@class, 'metrics')]");
                var state = metricsNode.SelectSingleNode("span[@class='statefilter']/a").InnerText;
                var replies = int.Parse(numberRegex.Match(metricsNode.SelectSingleNode("span[@class='replycount']").InnerText).Groups[0].Value);
                var views = int.Parse(numberRegex.Match(metricsNode.SelectSingleNode("span[@class='viewcount']").InnerText).Groups[0].Value);
                var lastPostNodes = metricsNode.SelectNodes("span[@class='lastpost']");
                var createByNode = lastPostNodes[0];
                var userNamePattern = new Regex(@"profile/([^\?]+)");
                var createdBy = HttpUtility.UrlDecode(userNamePattern.Match(createByNode.SelectSingleNode("a").Attributes["href"].Value).Groups[1].Value);
                var createdOnString = createByNode.SelectSingleNode("span[2]").InnerText;
                DateTime createdOn = ParseDateTimeString(createdOnString, referTimestamp);
                string lastPostBy = string.Empty;
                DateTime? lastPostOn = null;
                if (lastPostNodes.Count > 1)
                {
                    var attr = lastPostNodes[1].SelectSingleNode("a[2]").Attributes["href"];
                    lastPostBy = attr == null
                        ? ""
                        : HttpUtility.UrlDecode(userNamePattern.Match(attr.Value).Groups[1].Value);
                    string lastPostOnString = lastPostNodes[1].SelectSingleNode("span[1]").InnerText;
                    lastPostOn = ParseDateTimeString(lastPostOnString, referTimestamp);
                }
                var frumCrumbNode = node.SelectSingleNode("div/div[@data-forumdetailsurl]");
                var forumDetailUrl = frumCrumbNode.Attributes["data-forumdetailsurl"].Value;
                var forum = new Regex("forum=(.+)", RegexOptions.IgnoreCase).Match(forumDetailUrl).Groups[1].Value;
                if(createdOn < startTimestamp)
                {
                    return list;
                }
                list.Add(new MsdnQuestionIndexEntity
                {
                    Id = id,
                    Title = HttpUtility.HtmlDecode(title),
                    CreatedBy = createdBy,
                    CreatedOn = createdOn,
                    LastPostBy = lastPostBy,
                    LastPostOn = lastPostOn,
                    Uri = uri,
                    Replies = replies,
                    Views = views,
                    State = state,
                    Forum = forum
                });
            }
            return list;
        }
        public static DateTime ParseDateTimeString(string dateTimeString, DateTime refer)
        {
            DateTime datetime;
            bool success = DateTime.TryParse(dateTimeString, out datetime);
            if (!success)
            {
                int minutes = 0;
                int hours = 0;
                var minutesPattern = new Regex(@"(\d+) minute");
                var hoursPattern = new Regex(@"(\d+) hour");
                var minutesMatch = minutesPattern.Match(dateTimeString);
                if (minutesMatch.Success)
                {
                    minutes = int.Parse(minutesMatch.Groups[1].Value);
                }
                var hoursMatch = hoursPattern.Match(dateTimeString);
                if (hoursMatch.Success)
                {
                    hours = int.Parse(hoursMatch.Groups[1].Value);
                }
                datetime = refer.Add(new TimeSpan(-hours, -minutes, 0));
            }
            return datetime;
        }

    }
}
