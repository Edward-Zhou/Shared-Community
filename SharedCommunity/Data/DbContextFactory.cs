using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Data
{
    public interface IDbContextFactory
    {
        ApplicationDbContext CreateDbContext();
    }
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceProvider _provider;
        public DbContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        public ApplicationDbContext CreateDbContext()
        {
            //using (IServiceScope scope = _provider.CreateScope())
            //{
            //    return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //}
            return _provider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
        

    }
}
