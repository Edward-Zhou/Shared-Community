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
                options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
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
            //Forum services
            services.AddScoped<ICommand, DownloadMSDNQuestions>((conn)=> new DownloadMSDNQuestions(Configuration.GetConnectionString("LocalConnection")));
            //dapper
            services.AddScoped<DbConnection>( _ => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            AuthConfigure.AddJwtBearer(services, Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
