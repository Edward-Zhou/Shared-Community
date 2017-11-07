using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedCommunity.Extensions;
using SharedCommunity.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace SharedCommunity.Data
{
    public class DataSeed
    {
        private static string Password = "1qaz@WSX";

        public static async Task InitializeData(IServiceProvider serviceProvider, bool createUsers = true)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                await InsertInitData(serviceScope.ServiceProvider, createUsers);
            }
        }

        private static async Task InsertInitData(IServiceProvider serviceProvider, bool createUsers)
        {
            //add users
            if(createUsers)
            {
                var superUser = GetUser("super@outlook.com");
                await CreateAdminUser(serviceProvider, superUser);
            }
            // add images
            //var images = GetImages();
            //await ModelExtensions.AddOrUpdateAsync(serviceProvider, image => image.Id, images);
        }
        private static async Task CreateAdminUser(IServiceProvider serviceProvider, ApplicationUser aUser)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetService<ApplicationDbContext>();

            var user = await userManager.FindByNameAsync(aUser.UserName);
            if (user == null)
            {
                await userManager.CreateAsync(aUser, Password);
            }            
        }
        private static ApplicationUser GetUser(string username)
        {
            return new ApplicationUser { UserName = username };
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
