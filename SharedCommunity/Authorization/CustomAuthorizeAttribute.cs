using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Authorization
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string Groups { get; set; }
    }
}
