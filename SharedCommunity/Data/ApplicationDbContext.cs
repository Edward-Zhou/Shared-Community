using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedCommunity.Models;

namespace SharedCommunity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //image tag
            builder.Entity<ImageTag>()
                .HasKey(k => new { k.ImageId, k.TagId});
            builder.Entity<ImageTag>()
                .HasOne(i => i.Image)
                .WithMany(it => it.ImageTags)
                .HasForeignKey(key => key.ImageId);
            builder.Entity<ImageTag>()
                .HasOne(t => t.Tag)
                .WithMany(it => it.ImageTags)
                .HasForeignKey(key=>key.TagId);
        }
    }
}
