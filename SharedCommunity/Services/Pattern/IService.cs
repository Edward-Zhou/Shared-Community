using SharedCommunity.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharedCommunity.Services.Pattern
{
    public interface  IService<TEntity> where TEntity: IObjectState
    {
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(params object[] keyValues);
        Task LogicDeleteAsync(params object[] keyValues);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(IEnumerable<TEntity> entities);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindByKeyAsync(string key);
        IQueryable<TEntity> Queryable();
        IEnumerable<TEntity> Filter(out int totalCount,
            Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null);
    }
}
