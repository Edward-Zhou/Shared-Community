using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Services.Pattern
{
    public class ServiceFactory
    {
    }
    public interface IMyServiceFactory
    {
        bool IsParent { get; set; }
        IMyService MyService();
    }
    public class MyServiceFactory : IMyServiceFactory
    {
        private readonly IServiceProvider provider;
        public bool IsParent { get; set; }
        public MyServiceFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }
        public IMyService MyService()
        {
            if (IsParent)
            {
                return provider.GetRequiredService<MyParent>();
            }

            return provider.GetRequiredService<MyChild>();
        }
    }
    public interface IMyService
    {
        string GetValue(int value);
    }

    public class MyParent : IMyService
    {
        public string GetValue(int value)
        {
            return "MyParent";
        }
    }
    public class MyChild : IMyService
    {
        public string GetValue(int value)
        {
            return "MyChild";
        }
    }
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var service = httpContext.RequestServices.GetRequiredService<IMyServiceFactory>();
            if (httpContext.Request.Path.Value.Contains("25"))
            {
                service.IsParent = true;
            }
            else
            {
                service.IsParent = false;
            }

            await _next(httpContext);
        }
    }
    public static class MyMiddlewareMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }

}
