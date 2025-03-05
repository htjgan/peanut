using Entity.User;
using EntityFrameworkCore;
using IRepository.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.User
{
    public class UserInfoRepository:BaseRepository<UserInfo>,IUserInfoRepository
    {
        public UserInfoRepository(MyDbContext dbContext) : base(dbContext) { }
    }
}
