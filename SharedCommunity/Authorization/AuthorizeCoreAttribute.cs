using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharedCommunity.Authorization
{
    public class AuthorizeCoreAttribute : TypeFilterAttribute
    {
        public AuthorizeCoreAttribute(string claimType, string claimValue) : base(typeof(AuthorizeCoreRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class AuthorizeCoreRequirementFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly JsonSerializerSettings _apiSerializerSettings;

        public AuthorizeCoreRequirementFilter(Claim claim, UserManager<ApplicationUser> userManager)
        {
            _claim = claim;
            _userManager = userManager;
            _apiSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = _claim.Value.Split(',');
            var userName = context.HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (_claim.Type == ClaimType.Role.ToString())
            {
                if (!_userManager.IsInRoleAsync(user, _claim.Value).Result)
                {
                    context.Result = new JsonResult(new WebApiResult { status = ApiStatus.NG, message = "用户权限不够" }, _apiSerializerSettings);
                }
            }
        }
    }

    public enum ClaimType
    {
        Group,
        Role
    }

}
