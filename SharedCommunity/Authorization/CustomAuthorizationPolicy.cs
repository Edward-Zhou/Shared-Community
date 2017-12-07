using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Authorization
{
    public class CustomAuthorizationPolicy : AuthorizationPolicy
    {
        public CustomAuthorizationPolicy(IEnumerable<IAuthorizationRequirement> requirements, IEnumerable<string> authenticationSchemes) : base(requirements, authenticationSchemes)
        {
        }
        public new static async Task<AuthorizationPolicy> CombineAsync(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authorizeData)
        {
            if (policyProvider == null)
            {
                throw new ArgumentNullException(nameof(policyProvider));
            }

            if (authorizeData == null)
            {
                throw new ArgumentNullException(nameof(authorizeData));
            }

            var policyBuilder = new CustomAuthorizationPolicyBuilder();
            var any = false;
            foreach (var authorizeDatum in authorizeData)
            {
                any = true;
                var useDefaultPolicy = true;
                if (!string.IsNullOrWhiteSpace(authorizeDatum.Policy))
                {
                    var policy = await policyProvider.GetPolicyAsync(authorizeDatum.Policy);
                    if (policy == null)
                    {
                        throw new InvalidOperationException("Resources.FormatException_AuthorizationPolicyNotFound(authorizeDatum.Policy)");
                    }
                    policyBuilder.Combine(policy);
                    useDefaultPolicy = false;
                }
                var rolesSplit = authorizeDatum.Roles?.Split(',');
                if (rolesSplit != null && rolesSplit.Any())
                {
                    var trimmedRolesSplit = rolesSplit.Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim());
                    policyBuilder.RequireRole(trimmedRolesSplit);
                    useDefaultPolicy = false;
                }
                var authTypesSplit = authorizeDatum.AuthenticationSchemes?.Split(',');
                if (authTypesSplit != null && authTypesSplit.Any())
                {
                    foreach (var authType in authTypesSplit)
                    {
                        if (!string.IsNullOrWhiteSpace(authType))
                        {
                            policyBuilder.AuthenticationSchemes.Add(authType.Trim());
                        }
                    }
                }
                if (useDefaultPolicy)
                {
                    policyBuilder.Combine(await policyProvider.GetDefaultPolicyAsync());
                }
            }
            return any ? policyBuilder.Build() : null;
        }
    }
}
