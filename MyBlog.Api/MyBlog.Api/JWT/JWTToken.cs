using Entity.User;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Api.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyBlog.Api.JWT
{
    public static class JWTToken
    {
        public static string GetJWTToken(UserInfo user,IConfiguration configuration)
        {
            // 创建JWT
            // 创建header,内部定义了JWT编码的算法
            var securityAlgorithm = SecurityAlgorithms.HmacSha256;
            // 创建payload,需要根据项目的需求来进行创建，例如可能会使用到用户的编号，用户名，邮箱等
            // 创建payload的内容，需要使用到Claim(每创建一个Claim对象，表示用户的一个信息)
            var claims = new[]
            {
                  // 第一项是，用户的ID，但是在JWT中，关于ID在JWT中有一个专用的名词叫做sub
                  new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,user.Id.ToString())
              };
            // 创建签名，签名会使用到私钥，可以把私钥保存在配置文件中
            // 读取配置文件中的私钥，然后转成字节
            var secretByte = Encoding.UTF8.GetBytes(configuration["Authentication:SecretKey"]!);
            // 使用加密算法，对私钥进行加密
            var signingKey = new SymmetricSecurityKey(secretByte);
            // 构建数字签名
            var signingCredentials = new SigningCredentials(signingKey, securityAlgorithm);
            // 构建token 内容
            var token = new JwtSecurityToken(
                // 谁发布的token数据，一般是服务端的地址
                issuer: configuration["Authentication:Issuer"],
                 // 把token数据发布给谁，一般就是前端项目，这里也可以填写服务端的地址，或者不填写也可以
                 audience: configuration["Authentication:Audience"],
                claims,
                // 发布时间
                notBefore: DateTime.Now,
                // 有效期
                expires: DateTime.Now.AddDays(1),                
                // 数字签名
                signingCredentials

                );
            // 将token生成字符串的形式进行输出
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
        }
    }
}
