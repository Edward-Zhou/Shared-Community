using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedCommunity.Authentication.JwtBearer;
using SharedCommunity.Helpers;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SharedCommunity.Authentication
{
    public class AuthConfigure
    {
        public const string AuthenticationScheme = "cookie"; 
        public static AuthConfigOptions authConfig { get; set; }
        public static void AddJwtBearer(IServiceCollection services, IConfiguration configuration )
        {
            authConfig = new AuthConfigOptions();
            configuration.GetSection("Authentication").Bind(authConfig);
            if (authConfig.JwtBearerConfig.IsEnabled)
            {
                ConfigureJwtBearerAuthentication(services);
            }
        }

        public static void UseJwtBearer(IApplicationBuilder app)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfig.JwtBearerConfig.SecurityKey));
            // Adding JWT generation endpoint
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(new TokenProviderOptions
            {
                Path = authConfig.JwtBearerConfig.Path,
                Issuer = authConfig.JwtBearerConfig.Issuer,
                Audience = authConfig.JwtBearerConfig.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                Expiration = TimeSpan.FromDays(authConfig.JwtBearerConfig.Expiration)
            }));
        }

        private static void ConfigureJwtBearerAuthentication(IServiceCollection services)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfig.JwtBearerConfig.SecurityKey));
            //these lines will disable Cookie authentication
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            services.AddAuthentication()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = authConfig.GoogleConfig.ClientId;
                googleOptions.ClientSecret = authConfig.GoogleConfig.ClientSecret;
                googleOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        var profileImg = context.User["image"].Value<string>("url");
                        identity.AddClaim(new Claim("profileImg", profileImg));
                        return Task.FromResult(0);
                    }
                };
            })
            .AddCookie()
            .AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.Events = new JwtBearerEvents {
                    OnTokenValidated = OnTokenValidated
                };
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,

                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = authConfig.JwtBearerConfig.Issuer,

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = authConfig.JwtBearerConfig.Audience,

                    // Validate the token expiry
                    ValidateLifetime = true,

                    // If you want to allow a certain amount of clock drift, set that here
                    ClockSkew = TimeSpan.Zero,

                    NameClaimType = "name"
                };
            });
        }

        private static Task OnTokenValidated(TokenValidatedContext context)
        {
            return Task.FromResult(0);
        }
    }
}
