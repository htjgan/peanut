using Entity.User;
using EntityFrameworkCore;
using IRepository;
using IRepository.User;
using IService;
using IService.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Api.Filters;
using Repository;
using Repository.User;
using Service;
using Service.User;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//使用内存缓存作为会话存储
builder.Services.AddDistributedMemoryCache();
//session注入
builder.Services.AddSession();
#region//DBContext注入
builder.Services.AddDbContext<MyDbContext>(option =>
{
    var conStr = builder.Configuration.GetConnectionString("conStr");
    option.UseSqlServer(conStr);
});
#endregion
#region//将Filter注入到容器
builder.Services.Configure<MvcOptions>(option =>
{
    option.Filters.Add<UnitOfWorkFilter>();
});
#endregion
#region//将仓储依赖注入
builder.Services.AddScoped<IBaseRepository<UserInfo>,BaseRepository<UserInfo>>();
builder.Services.AddScoped<IUserInfoRepository,UserInfoRepository>();
builder.Services.AddScoped<IBaseService<UserInfo>,BaseService<UserInfo>>();
builder.Services.AddScoped<IUserInfoService,UserInfoService>();
#endregion
#region//解决跨域问题
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});
#endregion
#region//AutoMapper注入
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion
#region//JWT认证服务注入
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // 获取配置文件中存储的密钥
    var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // 验证token的发布者
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        // 验证token的持有者
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Authentication:Audience"],
        // 验证toen是否过期
        ValidateLifetime = true,
        //表示token有效期到了立马生效，一般系统会默认再给五分钟
        ClockSkew = TimeSpan.Zero,
        // 使用私钥
        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
    };
    // --------------------------------------jwt 过期后触发该事件。
    options.Events = new JwtBearerEvents
    {
        // 授权失败
        OnAuthenticationFailed = context =>
        {
            // 错误的类型是token过期
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                /* context.Response.Headers.Add();*/

                context.Response.Headers.Add("Access-Control-Expose-Headers", "act");
                context.HttpContext.Response.Headers.Add("act", "expired");
                context.Response.Headers.AccessControlAllowOrigin = "*"; // 防止出现跨域的问题
            }
            return Task.CompletedTask;
        }
    };
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseSession();
//处理静态资源文件，如图片等
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
