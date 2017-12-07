using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Authorization
{
    public class CustomAuthorizationHandler : AuthorizationHandler<GroupsAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupsAuthorizationRequirement requirement)
        {
            var groups = requirement.AllowedGroups;
            return Task.FromResult(0);
        }
    }
}
