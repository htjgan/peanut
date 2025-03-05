using Entity.User;
using IService.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Api.Models;

namespace MyBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;
        public UserController(IUserInfoService userInfoService)
        {
            this._userInfoService = userInfoService;
        }
        /// <summary>
        /// 根据手机号获取登陆成功用户信息
        /// </summary>
        /// <param name="userPhone"></param>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo([FromQuery]string userPhone)
        {
            if (userPhone == null)
            {
                return BadRequest(new ApiResult<string>() { Success = false, Message = "手机号为空", Data = null });
            }
            var user = await _userInfoService.LoadEntities(a => a.UserPhone == userPhone).FirstOrDefaultAsync();    
            if (user == null)
            {
                return BadRequest(new ApiResult<string>() { Success = false, Message = "该用户未查询到", Data = null });
            }
            return Ok(new ApiResult<UserInfo> { Success = true, Message = "查询成功", Data = user });
        }
    }
}
