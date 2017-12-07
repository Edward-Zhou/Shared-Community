using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Authorization
{
    public class CustomAuthorizationPolicyBuilder: AuthorizationPolicyBuilder
    {
        public AuthorizationPolicyBuilder RequireGroup(params string[] groups)
        {
            if (groups == null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            return RequireRole((IEnumerable<string>)groups);
        }

        public AuthorizationPolicyBuilder RequireGroup(IEnumerable<string> groups)
        {
            if (groups == null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            Requirements.Add(new GroupsAuthorizationRequirement(groups));
            return this;
        }

    }
}
