using System.Security.Cryptography;
using System.Text;

namespace MyBlog.Api.Common
{
    public static class MD5Helper
    {
        /// <summary>
        /// 使用MD5加密将用户登录密码加密，保证安全
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string GetMD5(string pwd)
        {
            MD5 md5 = MD5.Create();
            byte[] buffers = Encoding.UTF8.GetBytes(pwd);
            byte[] result = md5.ComputeHash(buffers);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in result)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
