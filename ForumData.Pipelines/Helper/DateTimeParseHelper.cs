using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ForumData.Pipelines.Helper
{
    public class DateTimeParseHelper
    {
        public static DateTime? ConvertDateTime(string dateTimeString, DateType dateType, DateTime referTimestamp)
        {
            DateTime? datetime = null;
            if (dateType == DateType.DateTime)
            {
                datetime = DateTime.Parse(dateTimeString);
            }
            else if ( dateType == DateType.DateTimeString)
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
                datetime = referTimestamp.Add(new TimeSpan(-hours, -minutes, 0));
            }
            else if ( dateType == DateType.DateTimeLong)
            {
                var millisecond =new Regex(@"(\d+)");
                var millisecondMatch = millisecond.Match(dateTimeString);
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                datetime = (origin.AddSeconds((Convert.ToInt64(millisecondMatch.Groups[1].Value) / 1000)));
            }            
            return datetime;
        }
    }
    public enum DateType
    {
        DateTime,
        DateTimeString,
        DateTimeLong
    }
}
