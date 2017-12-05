using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCommunity.Extensions
{
    public class AspNetCoreModuleExceptionMiddleware
    {
        public static Action<IApplicationBuilder> OutPutException()
        {
            return errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    if (exception != null)
                    {
                        var exceptionJson = Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(exception.Error,
                            new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            })
                        );
                        context.Response.ContentType = "application/json";
                        await context.Response.Body.WriteAsync(exceptionJson, 0, exceptionJson.Length);
                    }
                });
            };
        }
    }
}
