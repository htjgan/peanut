using AutoMapper;
using Entity.User;
using MyBlog.Api.DTO;

namespace MyBlog.Api.AutoMapper
{
    public class UserInfoProfile:Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfo, UserInfoLoginDTO>();
        }
    }
}
