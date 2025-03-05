using Entity.User;
using IRepository.User;
using IService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.User
{
    public class UserInfoService:BaseService<UserInfo>,IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        public UserInfoService(IUserInfoRepository userInfoRepository)
        {
            base._repository = userInfoRepository;
           this. _userInfoRepository = userInfoRepository;
        }
    }
}
