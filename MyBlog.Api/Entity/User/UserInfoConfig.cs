using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.User
{
    public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
    {
        void IEntityTypeConfiguration<UserInfo>.Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("T_UserInfo");
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(20).HasDefaultValue("高级牛马");
            builder.Property(u => u.UserEmail).HasMaxLength(50);
            builder.Property(u => u.UserPassword).HasMaxLength(20).IsRequired();
            builder.Property(u => u.UserPhone).HasMaxLength(11);
            builder.Property(u => u.Gender).HasDefaultValue(0);
            builder.Property(u => u.PhotoUrl).HasMaxLength(100);
        }
    }
}
