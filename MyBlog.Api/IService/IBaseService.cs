using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda);
        IQueryable<TEntity> LoadPageEntities<S>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, S>> orderByLambda, out int totalCount, bool isAsc);
        Task<bool> AddEntityAsync(TEntity entity);
        Task<bool> UpdateEntityAsync(TEntity entity);
        Task<bool> DeleteEntityAsync(TEntity entity);
    }
}
