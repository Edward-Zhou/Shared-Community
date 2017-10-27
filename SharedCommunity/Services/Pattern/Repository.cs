using Microsoft.EntityFrameworkCore;
using SharedCommunity.Data;
using SharedCommunity.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LinqKit;
using SharedCommunity.Models.Entities;

namespace SharedCommunity.Services.Pattern
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity: EntityBase, IObjectState
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public DbSet<TEntity> DbSet => _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;

            var dbContext = context as DbContext;
            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
             await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task DeleteAsync(object key)
        {
            var entity = await FindAsync(key);
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Remove(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(
                () =>  {
                    entity.ObjectState = ObjectState.Deleted;
                    _dbSet.Remove(entity);
                }
            );
        }

        public virtual async Task LogicDeleteAsync(object key)
        {
            var entity = await FindAsync(key);
            await LogicDeleteAsync(entity);
        }

        public virtual async Task LogicDeleteAsync(TEntity entity)
        {
            entity.Deleted = true;
            entity.ModifiedDate = DateTime.Now;
            await UpdateAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => {
                entity.ModifiedDate = DateTime.Now;
                _dbSet.Update(entity);
            });
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => {
                _dbSet.UpdateRange(entities);
            });
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.FromSql(query, parameters).AsQueryable().AsNoTracking();
        }

        public IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        //IRepository<T> IRepository<TEntity>.GetRepository<T>()
        //{
        //    throw new NotImplementedException();
        //}

        public IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes !=null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query.AsExpandable().Where(filter);
            }
            if (page != null & pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        public async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> query = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            //See: Best Practices in Asynchronous Programming http://msdn.microsoft.com/en-us/magazine/jj991977.aspx
            return await Task.Run(() => Select(query, orderBy, includes, page, pageSize).AsEnumerable()).ConfigureAwait(false);
        }

    }
}
