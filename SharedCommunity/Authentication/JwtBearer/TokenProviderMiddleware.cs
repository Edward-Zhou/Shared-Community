using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharedCommunity.Authentication.JwtBearer
{
    /* This middleware is based on this article: https://stormpath.com/blog/token-authentication-asp-net-core
    **/
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;
        //private readonly UserManager<ApplicationUser> _userManager;


        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options)
        {
            _next = next;
            _options = options.Value;
            //_userManager = userManager;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            //if the request path doesn't match, skip
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }
            //Request must be Post with Content-Type : application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Request must be Post with Content-Type : application/x-www-form-urlencoded");
            }
            return GenerateToken(context, userManager);
        }

        private async Task GenerateToken(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            var identity = await GetIdentity(username, password, userManager);
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }
            var now = DateTime.UtcNow;
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            //Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //return user information
            var response = new {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };

            //Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }
        private async Task<ClaimsIdentity> GetIdentity(string username, string password, UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByNameAsync(username);
            var isValid = await userManager.CheckPasswordAsync(user, password);
            if (isValid)
            {
                return new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "Token"), new Claim[] { });
            }

            // Credentials are invalid, or account doesn't exist
            return null;
        }
    }
}
