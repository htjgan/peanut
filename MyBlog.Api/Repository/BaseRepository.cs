using EntityFrameworkCore;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepository<TEntity>:IBaseRepository<TEntity> where TEntity : class,new()
    {
        private readonly MyDbContext _dbContext;
        public BaseRepository(MyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteEntityAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return Task.FromResult(true);
        }

        public IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Where(expression);
        }

        public IQueryable<TEntity> LoadPageEntities<T>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, T>> expression1, out int totalCount, bool isAsc)
        {
            //获取所有数据
            var temp = _dbContext.Set<TEntity>().Where(expression);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy<TEntity,T>(expression1).Skip<TEntity>((pageIndex - 1) * pageSize).Take<TEntity>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<TEntity, T>(expression1).Skip<TEntity>((pageIndex - 1) * pageSize).Take<TEntity>(pageSize);
            }
            return temp;
        }

        public Task<bool> UpdateEntityAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return Task.FromResult(true);
        }
    }
}
