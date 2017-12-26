using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SharedCommunity.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;

namespace SharedCommunity.ViewModels
{
    public class ResponseFormatter : TextOutputFormatter
    {
        public ResponseFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            //check api resposne
            if(context.HttpContext.Request.Path.Value.Contains("api"))
            {
                WebApiResult result = new WebApiResult { code = response.StatusCode.ToString(), data = context.Object };
                return response.WriteAsync(JsonConvert.SerializeObject(result));
                //return Task.Run(() => result);
            }
            return Task.FromResult(0);
        }
    }
}
