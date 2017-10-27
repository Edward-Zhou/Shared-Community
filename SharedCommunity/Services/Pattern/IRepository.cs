using Microsoft.EntityFrameworkCore;
using SharedCommunity.Models.Entities;
using SharedCommunity.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharedCommunity.Services.Pattern
{
    public interface IRepository<TEntity> where TEntity : EntityBase, IObjectState
    {
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(object key);
        Task DeleteAsync(TEntity entity);
        Task LogicDeleteAsync(object key);
        Task LogicDeleteAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> FindAsync(params object[] keyValues);
        DbSet<TEntity> DbSet { get; }
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        IQueryable<TEntity> Queryable();
        //IRepository<T> GetRepository<T>() where T : class, IObjectState;
        IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null);
        Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> query = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null);
    }
}
