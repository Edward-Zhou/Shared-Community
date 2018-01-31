using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedCommunity.Tasks
{
    public class TestBatch : IHostedService
    {
        // private MyDbContext bdd;
        public TestBatch(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                //bdd = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            }
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
                Console.WriteLine("Test");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // throw new System.NotImplementedException();
        }

    }
}
