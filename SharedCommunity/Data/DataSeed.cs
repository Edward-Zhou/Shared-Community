using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedCommunity.Extensions;
using SharedCommunity.Models.Entities;

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
            // add images
            var images = GetImages();
            await ModelExtensions.AddOrUpdateAsync(serviceProvider, image => image.Id, images);
        }

        private static IEnumerable<ApplicationUser> GetUsers() {
            var users = new ApplicationUser[] {
                new ApplicationUser{ UserName="super" },
                new ApplicationUser{ UserName="admin" },
                new ApplicationUser{ UserName="normal" }
            };
            return users;
        }

        private static IEnumerable<Image> GetImages() {
            var images = new Image[] {
                new Image{ Name = "Image1"},
                new Image{ Name = "Image2"},
                new Image{ Name = "Image3"}
            };
            return images;
        }
    }
}
