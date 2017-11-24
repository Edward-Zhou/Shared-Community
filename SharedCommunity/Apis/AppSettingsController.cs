using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedCommunity.Extensions;
using SharedCommunity.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SharedCommunity.Data;

namespace SharedCommunity.Apis
{
    [Produces("application/json")]
    [Route("api/AppSettings")]
    public class AppSettingsController : Controller
    {
        private readonly IWritableOptions<ConstConfigOptions> _options;
        private readonly IServiceProvider _provider;
        public AppSettingsController(IWritableOptions<ConstConfigOptions> options, IServiceProvider provider)
        {
            _options = options;
            _provider = provider;
        }

        //Create New DB
        //reference https://stackoverflow.com/questions/45384751/how-to-use-mssqllocaldb-in-entity-framework-core-1-1
        public async Task<IActionResult> CreateDb()
        {
            string connectionString = @"Data Source=10.168.172.107;Initial Catalog=CoreDbOptions;Integrated Security=False;User ID=sa;Password=123456;";
            var serviceProvider = new ServiceCollection()
                                   .AddEntityFrameworkSqlServer()
                                   .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseSqlServer(connectionString)
                    .UseInternalServiceProvider(serviceProvider);

            var context = new ApplicationDbContext(builder.Options);
            context.Database.Migrate();

            return Ok();
        }

    }
}