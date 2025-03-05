using Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository.User
{
    public interface IUserInfoRepository:IBaseRepository<UserInfo>
    {
        //这里定义自己独有的数据访问的方法
    }
}
