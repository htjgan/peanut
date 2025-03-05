using Entity.User;
using IRepository;
using IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BaseService<UserInfo> :IBaseService<UserInfo> where UserInfo : class,new()
    {
        public IBaseRepository<UserInfo> _repository;
        public Task<bool> AddEntityAsync(UserInfo entity)
        {
            return _repository.AddEntityAsync(entity);
        }

        public Task<bool> DeleteEntityAsync(UserInfo entity)
        {
            return _repository.DeleteEntityAsync(entity);
        }

        public IQueryable<UserInfo> LoadEntities(Expression<Func<UserInfo, bool>> whereLambda)
        {
            return _repository.LoadEntities(whereLambda);
        }

        public IQueryable<UserInfo> LoadPageEntities<S>(int pageIndex, int pageSize, Expression<Func<UserInfo, bool>> whereLambda, Expression<Func<UserInfo, S>> orderByLambda, out int totalCount, bool isAsc)
        {
            return _repository.LoadPageEntities(pageIndex, pageSize, whereLambda, orderByLambda, out totalCount, isAsc);
        }

        public Task<bool> UpdateEntityAsync(UserInfo entity)
        {
            return _repository.UpdateEntityAsync(entity);
        }
    }
}
