using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Data;
using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SharedCommunity.XunitTest
{
    public class ServiceProviderFixture
    {
        private static bool isSetupData = false;
        private readonly IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider;
        public ServiceProviderFixture()
        {
            var services = new ServiceCollection();
#if true
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<ApplicationDbContext>(db => db.UseInMemoryDatabase("SharedCommunity").UseInternalServiceProvider(efServiceProvider));
#else
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer("Server=10.168.172.107;Database=SharedCommunity;Trusted_Connection=True;MultipleActiveResultSets=true");
            }); 
#endif
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IHostingEnvironment, HostingEnvironment>();

            services.AddMvc();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new AutofacModule());

            var container = builder.Build();
            _serviceProvider = new AutofacServiceProvider(container);
            //Initialize Db
            PopulateData(_serviceProvider);
        }
        public HttpContext GetHttpContext(string userName)
        {
            var httpContext = new DefaultHttpContext();
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
            httpContext.RequestServices = _serviceProvider;
            return httpContext;
        }
        private void PopulateData(IServiceProvider serviceProvider)
        {
            if (!isSetupData)
            {
                DataSeed.InitializeData(serviceProvider, true).Wait();
                isSetupData = true;
            }
        }
    }
}
