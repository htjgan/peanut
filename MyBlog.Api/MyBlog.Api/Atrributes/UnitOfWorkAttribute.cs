namespace MyBlog.Api.Atrributes
{
    //特性，统一在这儿保存数据库操作
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute:Attribute
    {
        public Type[] DbContextTypes {  get; set; }
        public UnitOfWorkAttribute(Type[] dbContextTypes ) 
        {
            this.DbContextTypes = dbContextTypes;
        }
    }
}
