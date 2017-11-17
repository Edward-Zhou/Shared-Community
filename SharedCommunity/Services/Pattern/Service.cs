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
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : EntityBase, IObjectState
    {
        public readonly IRepository<TEntity> _repository;

        protected Service(IRepository<TEntity> repository) => _repository = repository;

        public virtual async Task DeleteAsync(params object[] keyValues) => await _repository.DeleteAsync(keyValues);

        public virtual async Task LogicDeleteAsync(params object[] keyValues) => await _repository.LogicDeleteAsync(keyValues);

        public virtual async Task<TEntity> FindAsync(params object[] keyValues) => await _repository.FindAsync(keyValues);

        public virtual async Task<TEntity> FindByKeyAsync(string key) => await _repository.DbSet.Where(entity => entity.Name == key && entity.Deleted ==false).FirstOrDefaultAsync();

        public virtual async Task InsertAsync(TEntity entity) => await _repository.InsertAsync(entity);

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities) => await _repository.InsertRangeAsync(entities);


        public IQueryable<TEntity> Queryable() =>  _repository.Queryable();


        public virtual async Task UpdateAsync(TEntity entity) => await _repository.UpdateAsync(entity);


        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities) => await _repository.UpdateRangeAsync(entities);

        public virtual IEnumerable<TEntity> Filter(
            out int totalCount,
            Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            totalCount = _repository.DbSet.Where(where).Count();
            return _repository.Select(where, orderBy, includes, page, pageSize);
        }

    }
}
