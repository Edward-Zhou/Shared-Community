using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedCommunity.Extensions;

namespace SharedCommunity.Data
{
    public class DataSeed
    {
        public static async Task InitializeData(IServiceProvider serviceProvider, bool createUsers = true)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                await InsertInitData(serviceProvider, createUsers);
            }
        }

        private static async Task InsertInitData(IServiceProvider serviceProvider, bool createUsers)
        {
            //add users
            var users = GetUsers();
            await ModelExtensions.AddOrUpdateAsync(serviceProvider, u => u.Id, users);
        }

        private static IEnumerable<ApplicationUser> GetUsers() {
            var users = new ApplicationUser[] {
                //new ApplicationUser{ UserName="super" },
                //new ApplicationUser{ UserName="admin" },
                //new ApplicationUser{ UserName="normal" },
                new ApplicationUser{ Id="c0a5783b-3224-4884-b36e-9270a917f204", UserName="Test"}
            };
            return users;
        }
    }
}
