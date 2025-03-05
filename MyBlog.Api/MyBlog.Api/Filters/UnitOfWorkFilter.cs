using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MyBlog.Api.Atrributes;
using System.Reflection;

namespace MyBlog.Api.Filters
{
    /// <summary>
    /// 过滤器，在这里统一判断方法前是否有对应特性，有则保存数据库操作
    /// </summary>
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if(result.Exception != null)
            {
                return;
            }
            //先去执行构造器中的方法，返回时查看是否有对应特性，有则进行保存数据库操作
            var controllerActionDesc = context.ActionDescriptor as ControllerActionDescriptor;
            if(controllerActionDesc == null)
            {
                return;
            }
            var unitAttr = controllerActionDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            if(unitAttr == null)
            {
                return;
            }
            foreach(var dcContext in unitAttr.DbContextTypes)
            {
                var db = context.HttpContext.RequestServices.GetService(dcContext) as DbContext;
                if(db != null)
                {
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
