using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Models
{
    public class ApiStatus
    {
        public static string OK = "OK";
        public static string NG = "NG";
    }
    public class WebApiResult
    {
        public string status { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public object data { get; set; }
    }
}
