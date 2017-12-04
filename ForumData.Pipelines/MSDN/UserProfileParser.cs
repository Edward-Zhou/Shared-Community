using ForumData.Pipelines.Helper;
using ForumData.Pipelines.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ForumData.Pipelines.MSDN
{
    public class UserProfileParser
    {
        private const string URL_FORMAT = "https://social.msdn.microsoft.com/Profile/{0}/activity";
        private static HttpDonwloadAgent _agent;
        static UserProfileParser()
        {
            _agent = new HttpDonwloadAgent();
        }
        public static string GenerateRequest(string UserName)
        {
            string request = string.Format(URL_FORMAT, UserName);
            return request;
        }
        public static MsdnQuestionIndexEntity Parse(MsdnQuestionIndexEntity entity)
        {
            string html = _agent.GetString(GenerateRequest(entity.CreatedBy));
            entity.LastActiveOn = LastActiveOn(html);
            return entity;
        }
        public static DateTime? LastActiveOn(string html)
        {
            try
            {
                return LastActiveOn(html, DateTime.Now);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static DateTime? LastActiveOn(string html, DateTime referTimestamp)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='activity-date']");
            if (node == null)
            {
                return null;
            }
            return DateTimeParseHelper.ConvertDateTime(node.Attributes["title"].Value,DateType.DateTimeLong, referTimestamp);
        }

    }
}
