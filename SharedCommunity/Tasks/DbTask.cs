using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Data;
using SharedCommunity.Tasks.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedCommunity.Tasks
{
    public class DbTask: BackgroundService
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _provider;
        public DbTask(IDbContextFactory dbContextFactory, IServiceProvider serviceProvider)
        {
            _context = dbContextFactory.CreateDbContext();
            _provider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _provider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userList = context.Users.ToList();
                }
                var users = _context.Users.ToList();

                await Task.Delay(-1, stoppingToken);
            }
        }
    }
}
