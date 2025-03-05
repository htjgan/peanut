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
//ʹ���ڴ滺����Ϊ�Ự�洢
builder.Services.AddDistributedMemoryCache();
//sessionע��
builder.Services.AddSession();
#region//DBContextע��
builder.Services.AddDbContext<MyDbContext>(option =>
{
    var conStr = builder.Configuration.GetConnectionString("conStr");
    option.UseSqlServer(conStr);
});
#endregion
#region//��Filterע�뵽����
builder.Services.Configure<MvcOptions>(option =>
{
    option.Filters.Add<UnitOfWorkFilter>();
});
#endregion
#region//���ִ�����ע��
builder.Services.AddScoped<IBaseRepository<UserInfo>,BaseRepository<UserInfo>>();
builder.Services.AddScoped<IUserInfoRepository,UserInfoRepository>();
builder.Services.AddScoped<IBaseService<UserInfo>,BaseService<UserInfo>>();
builder.Services.AddScoped<IUserInfoService,UserInfoService>();
#endregion
#region//�����������
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
#region//AutoMapperע��
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion
#region//JWT��֤����ע��
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // ��ȡ�����ļ��д洢����Կ
    var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // ��֤token�ķ�����
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        // ��֤token�ĳ�����
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Authentication:Audience"],
        // ��֤toen�Ƿ����
        ValidateLifetime = true,
        //��ʾtoken��Ч�ڵ���������Ч��һ��ϵͳ��Ĭ���ٸ������
        ClockSkew = TimeSpan.Zero,
        // ʹ��˽Կ
        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
    };
    // --------------------------------------jwt ���ں󴥷����¼���
    options.Events = new JwtBearerEvents
    {
        // ��Ȩʧ��
        OnAuthenticationFailed = context =>
        {
            // �����������token����
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                /* context.Response.Headers.Add();*/

                context.Response.Headers.Add("Access-Control-Expose-Headers", "act");
                context.HttpContext.Response.Headers.Add("act", "expired");
                context.Response.Headers.AccessControlAllowOrigin = "*"; // ��ֹ���ֿ��������
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
//����̬��Դ�ļ�����ͼƬ��
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
