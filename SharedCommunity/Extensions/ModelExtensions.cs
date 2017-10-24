using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Extensions
{
    public class ModelExtensions
    {
        public static async Task<TEntity> AddOrUpdateAsync<TEntity>(
            IServiceProvider serviceProvider,
            Func<TEntity, object> propertyToMatch,
            TEntity entity) where TEntity : class
        {
            List<TEntity> existingData;
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                existingData = db.Set<TEntity>().ToList();
            }

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var state = existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(entity))) ? EntityState.Modified : EntityState.Added;

                if (state == EntityState.Added)
                {
                    await db.Set<TEntity>().AddAsync(entity);
                    await db.SaveChangesAsync();
                    return entity;
                }
                else
                {
                    if (GetConcurrencyStamp(entity)!=null)
                    {
                        var existingEntity = existingData.Where(g => propertyToMatch(g).Equals(propertyToMatch(entity))).FirstOrDefault();
                        SetConcurrencyStamp(existingEntity, entity);
                    }
                    db.Set<TEntity>().Update(entity);
                    db.SaveChanges();
                    return entity;
                }
            }
        }

        public static async Task<IEnumerable<TEntity>> AddOrUpdateAsync<TEntity>(
            IServiceProvider serviceProvider,
            Func<TEntity, object> propertyToMatch, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                await AddOrUpdateAsync(serviceProvider, propertyToMatch, entity);
            }
            return entities;
        }
        public static Guid? GetConcurrencyStamp<TEntity>(TEntity entity) where TEntity : class
        {            
            var property = entity.GetType().GetProperty("ConcurrencyStamp");
            if (property != null)
            {
                return new Guid(property.GetValue(entity).ToString());
            }
            return null;
        }
        public static TEntity SetConcurrencyStamp<TEntity>(TEntity source, TEntity target) where TEntity : class
        {
            var property = source.GetType().GetProperty("ConcurrencyStamp");
            var value = property.GetValue(source);
            property.SetValue(target, value, null);
            return target;            
        }

    }
}
