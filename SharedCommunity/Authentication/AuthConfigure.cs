using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedCommunity.Helpers;
using System;
using System.Text;

namespace SharedCommunity.Authentication
{
    public class AuthConfigure
    {
        public const string AuthenticationScheme = "cookie";        
        public static void AddJwtBearer(IServiceCollection services, IConfiguration configuration )
        {
            var authConfig = new AuthConfigOptions();
            configuration.GetSection("Authentication").Bind(authConfig);
            if (authConfig.JwtBearerConfig.IsEnabled)
            {
                ConfigureJwtBearerAuthentication(services, authConfig);
            }
        }

        public static void UseJwtBearer(IApplicationBuilder app, AuthConfigOptions authConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfig.JwtBearerConfig.SecurityKey));
            // Adding JWT generation endpoint
            //app.UseJwtBearerAuthentication();
        }

        private static void ConfigureJwtBearerAuthentication(IServiceCollection services, AuthConfigOptions authConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfig.JwtBearerConfig.SecurityKey));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
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
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
