namespace MyBlog.Api.DTO
{
    public class UserInfoLoginDTO
    {
        /// <summary>
        /// 电话
        /// </summary>
        public string? UserPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? UserPassword { get; set; }
        /// <summary>
        /// 是否保存登录状态
        /// </summary>
        public bool rememberMe {  get; set; }
    }
}
