using Microsoft.AspNetCore.Authorization;
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
        public AuthorizeCoreAttribute(string Groups="", string Roles="", string Permissions="", string Claims="") : base(typeof(AuthorizeCoreRequirementFilter))
        {
            Arguments = new object[] { new Author { Groups = Groups, Roles = Roles, Permissions = Permissions, Claims = Claims } };
        }
    }
    public class AuthorizeCoreRequirementFilter : IAuthorizationFilter
    {
        private readonly Author _author;
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly JsonSerializerSettings _apiSerializerSettings;

        public AuthorizeCoreRequirementFilter(Author author, UserManager<ApplicationUser> userManager)
        {
            _author = author;
            _userManager = userManager;
            _apiSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isGroupsAuthorized = true;
            bool isRolesAuthorized = true;
            bool isPermissionAuthorized = true;
            bool isClaimsAhthorized = true;
            var user = _userManager.GetUserAsync(context.HttpContext.User).Result;
            if (user == null)
            {
                context.Result = new JsonResult(new WebApiResult { status = ApiStatus.NG, message = "用户权限不够" }, _apiSerializerSettings);
                return;
            }
            var groupsSplit = _author.Groups?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
            if(groupsSplit !=null && groupsSplit.Any())
            {
                isGroupsAuthorized = true;
            }
            var rolesSplit = _author.Roles?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r=>r.Trim());
            if (rolesSplit !=null && rolesSplit.Any())
            {
                isRolesAuthorized = rolesSplit.Any(role=> _userManager.IsInRoleAsync(user, role).Result);
            }
            var claimsSplit = _author.Claims?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim());
            if(claimsSplit != null && claimsSplit.Any())
            {
                isClaimsAhthorized = claimsSplit.Any(claim => context.HttpContext.User.Claims.Any(uc => uc.Type == claim.ToString()) == true);
            }
            var permissonsSplit = _author.Permissions?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
            if (permissonsSplit != null && permissonsSplit.Any())
            {
                isPermissionAuthorized = true;
            }
            if (isGroupsAuthorized==false || isRolesAuthorized==false ||isPermissionAuthorized==false)
            {
                context.Result = new JsonResult(new WebApiResult { status = ApiStatus.NG, message = "用户权限不够" }, _apiSerializerSettings);
            }

        }
    }
    public class Author
    {
        public string Groups { get; set; }
        public string Roles { get; set; }
        public string Permissions { get; set; }
        public string Claims { get; set; }
    }
}
