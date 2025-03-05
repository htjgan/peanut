using Entity.User;
using IRepository.User;
using IService.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Api.DTO;
using MyBlog.Api.JWT;
using MyBlog.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;
        private readonly IConfiguration _configuration;
        public LoginController(IUserInfoService userInfoService, IConfiguration configuration)
        {
            this._userInfoService = userInfoService;
            this._configuration = configuration;
        }
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="userInfoLoginDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UserLogin(UserInfoLoginDTO userInfoLoginDto)
        {
            if (string.IsNullOrEmpty(userInfoLoginDto.UserPhone) || string.IsNullOrEmpty(userInfoLoginDto.UserPassword))
            {
                return BadRequest(new ApiResult<string>() { Success = false, Message = "手机号或密码为空", Data = null });
            }
            // 从缓存（Session）中获取用户信息
            var sessionKey = userInfoLoginDto.UserPhone;
            var sessionValue = HttpContext.Session.GetString(sessionKey);
            UserInfo userInfo = null;
            if (string.IsNullOrEmpty(sessionValue))
            {
                // 缓存中没有用户信息，从数据库中查找
                userInfo = await _userInfoService.LoadEntities(u => u.UserPhone == userInfoLoginDto.UserPhone && u.UserPassword == userInfoLoginDto.UserPassword)
                    .FirstOrDefaultAsync();
                if (userInfo != null)
                {
                    // 将用户信息保存到缓存（Session）中
                    HttpContext.Session.SetString(sessionKey, JsonConvert.SerializeObject(userInfo));
                }
            }
            else
            {
                // 缓存中有用户信息，但尝试反序列化
                try
                {
                    userInfo = JsonConvert.DeserializeObject<UserInfo>(sessionValue);
                }
                catch (Exception)
                {
                    // 反序列化失败，可能由于数据损坏，视为缓存无效，从数据库中重新查找
                    userInfo = null;
                }
            }
            // 验证用户是否存在
            if (userInfo == null)
            {
                return BadRequest(new ApiResult<string> { Success = false, Message = "手机号或密码错误", Data = null });
            }
            // 获取JWT令牌
            var token = JWTToken.GetJWTToken(userInfo, _configuration);
            // 返回登录成功的响应
            return Ok(new ApiResult<string> { Success = true, Message = "登录成功，已保存用户登录状态", Data = token });
        }
    }
}
