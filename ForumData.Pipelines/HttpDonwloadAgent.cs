using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ForumData.Pipelines
{
    public class HttpDonwloadAgent
    {
        private const int DEFAULT_MAX_TRY_TIMES = 3;
        private const int TRY_AFTER_SECONDS = 1;

        private static int _maxTryTimes = DEFAULT_MAX_TRY_TIMES;

        public static int MaxTryTimes
        {
            get
            {
                return _maxTryTimes;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("max_try_times must be greater than 0");
                }

                _maxTryTimes = value;
            }
        }

        private static HttpClient _client;

        public HttpDonwloadAgent()
        {
            ServicePointManager.DefaultConnectionLimit = 20;
            _client = new HttpClient();

            var headers = _client.DefaultRequestHeaders;
            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            headers.Add("Cache-Control", "keep-alive");
            headers.Add("Accept-Language", "en-US,en;q=0.8,zh-Hans-CN;q=0.5,zh-Hans;q=0.3");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; rv:16.0) Gecko/20100101 Firefox/16.0");

        }

        public static async Task<string> GetString(string url)
        {
           
            // the code that you want to measure comes here
            
            string s= await GetString(url, Encoding.UTF8);
            
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            return s;
        }

        public static async Task<string> GetString(string url, Encoding encoding)
        {
            using (var stream = await GetStream(url))
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static async Task<Stream> GetStream(string url)
        {
            Exception ex = null;

            for (int i = 0; i < _maxTryTimes; i++)
            {
                try
                {
                    return await _client.GetStreamAsync(url);
                }
                catch (Exception e)
                {
                    System.Threading.Thread.Sleep(TRY_AFTER_SECONDS * 1000);
                    ex = e;
                }
            }

            throw ex;
        }

        public HttpResponseMessage GetResponseCode(string url)
        {
            var response = _client.GetAsync(url);

            return response.Result;
        }
    }
}
