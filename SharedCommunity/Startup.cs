using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Data;
using SharedCommunity.Models;
using SharedCommunity.Services;
using SharedCommunity.Helpers;
using SharedCommunity.Services.Pattern;
using SharedCommunity.Models.Entities;
using SharedCommunity.Authentication;
using SharedCommunity.Extensions;
using Kivi.Platform.Core.SDK;
using ForumData.Pipelines;
using SharedCommunity.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;
using System.Data.SqlClient;
using System;
using SharedCommunity.ViewModels;
using SharedCommunity.Tasks.Pattern;
using SharedCommunity.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace SharedCommunity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }   
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add options service to configure appsettings
            services.AddOptions();
            services.Configure<ConstConfigOptions>(Configuration.GetSection("ConstConfig"));
            services.Configure<AuthConfigOptions>(Configuration.GetSection("Authentication"));
            services.ConfigureWritable<AuthConfigOptions>(Configuration.GetSection("ConstConfig"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IDbContextFactory, DbContextFactory>();
            //Add CORS
            services.AddCors(options => {
                options.AddPolicy("AllowAllOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            //Authorize
            //services.AddAuthorization(options =>
            //{

            //    options.AddPolicy("grou", policy => new CustomAuthorizationPolicyBuilder().RequireGroup());
            //});
            services.AddSingleton<IAuthorizationHandler,CustomAuthorizationHandler>();
            //Add own services
            services.AddScoped<IRepository<Image>, Repository<Image>>();
            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<IMyServiceFactory, MyServiceFactory>();
            services.AddScoped<MyParent>();
            services.AddScoped<MyChild>();

            //Forum services
            services.AddScoped<ICommand, DownloadMSDNQuestions>((conn)=> new DownloadMSDNQuestions(Configuration.GetConnectionString("DefaultConnection")));
            //dapper
            services.AddScoped<DbConnection>( _ => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            AuthConfigure.AddJwtBearer(services, Configuration);
            //X-CSRF-Token add cookie by with withCredentials and X-XSRF-TOKEN
            //http://www.dotnetcurry.com/aspnet/1343/aspnet-core-csrf-antiforgery-token
            //https://github.com/mgonto/restangular/issues/243
            services.AddAntiforgery(options=> {
                options.HeaderName = "X-XSRF-Token";
                //options.SuppressXFrameOptionsHeader = false;
            });
            services.AddMvc(options=> {
                //options.OutputFormatters.Insert(0, new ResponseFormatter());
            });

            //add scheduled tasks 
            //services.AddSingleton<IScheduledTask, ThreadTask>();
            //services.AddSingleton<IHostedService, DbTask>();
            ////services.AddSingleton<IHostedService, TestBatch>();
            //services.AddScheduler((sender, args) =>
            //{
            //    Console.Write(args.Exception.Message);
            //    args.SetObserved();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            var configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .Build();
            string environment = configuration.GetSection("SharedCommunity").Value;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //X-CSRF-TOKEN
            //app.Use(next => context =>
            //{
            //    string path = context.Request.Path.Value;
            //    if (
            //        string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
            //        string.Equals(path, "/api/MSDNForum/MSDNThreadDownload", StringComparison.OrdinalIgnoreCase) ||
            //        string.Equals(path, "/index.html", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // We can send the request token as a JavaScript-readable cookie, 
            //        // and Angular will use it by default.
            //        var tokens = antiforgery.GetAndStoreTokens(context);
            //        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //            new CookieOptions() { HttpOnly = false });
            //    }

            //    return next(context);
            //});
            app.UseStaticFiles();
            app.UseCors("AllowAllOrigin");

            app.UseAuthentication();


            AuthConfigure.UseJwtBearer(app);

            //configure jwt
            app.UseExceptionHandler(AspNetCoreModuleExceptionMiddleware.OutPutException());
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //seed init data
            //DataSeed.InitializeData(app.ApplicationServices).Wait();
        }
    }
}
