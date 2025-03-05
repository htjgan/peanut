using Entity.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore
{
    public class MyDbContext:DbContext
    {
        public DbSet<UserInfo> userInfos {  get; set; }

        //通过构造函数让容器来帮忙构建实例，保证全局都使用同一个DbContext
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //因为不在同一个项目中，所以需要通过反射去获取对应的配置类
            var assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "Entity.dll");
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(type.Assembly);
            }
        }
    }
}
