using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Authorization
{
    public class GroupsAuthorizationRequirement : AuthorizationHandler<GroupsAuthorizationRequirement>, IAuthorizationRequirement
    {
        public GroupsAuthorizationRequirement(IEnumerable<string> allowedGroups)
        {
            if (allowedGroups == null)
            {
                throw new ArgumentNullException(nameof(allowedGroups));
            }

            if (allowedGroups.Count() == 0)
            {
                throw new InvalidOperationException("Group is Empty");
            }
            AllowedGroups = allowedGroups;
        }

        /// <summary>
        /// Gets the collection of allowed roles.
        /// </summary>
        public IEnumerable<string> AllowedGroups { get; }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupsAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                bool found = false;
                if (requirement.AllowedGroups == null || !requirement.AllowedGroups.Any())
                {
                    // Review: What do we want to do here?  No roles requested is auto success?
                }
                else
                {
                    found = requirement.AllowedGroups.Any(r => context.User.IsInRole(r));
                }
                if (found)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
