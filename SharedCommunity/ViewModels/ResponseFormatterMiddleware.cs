using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.ViewModels
{
    public class ResponseFormatterMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseFormatterMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            return _next(context);
        }
    }
}
