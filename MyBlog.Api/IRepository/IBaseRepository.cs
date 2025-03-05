using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class,new()//表示TEntity是一个类且有一个无参的构造函数
    {
        //根据条件查询数据
        IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> expression);
        //分页查询
        IQueryable<TEntity> LoadPageEntities<T>(int pageIndex , int pageSize , Expression<Func<TEntity,bool>> expression , Expression<Func<TEntity,T>> expression1 , out int totalCount , bool isAsc);
        //添加数据
        Task<bool> AddEntityAsync(TEntity entity);
        //修改数据
        Task<bool> UpdateEntityAsync(TEntity entity);
        //删除数据
        Task<bool> DeleteEntityAsync(TEntity entity);
    }
}
