using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> b)
    {
        b.ToTable("Admins", tb => tb.HasComment("管理员表"));

        b.HasKey(s => s.Id).HasName("PRIMARY"); 
        b.HasIndex(e => e.UserName).IsUnique();

        b.Property(e => e.Id)
            .HasComment("ID");
        b.Property(e => e.Avatar)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("头像");
        b.Property(e => e.Email)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("电子邮箱");
        b.Property(e => e.LoginFailure)
            .HasComment("失败次数");
        b.Property(e => e.LoginIp)
            .HasMaxLength(50)
            .HasComment("登录IP");
        b.Property(e => e.LoginTime)
            .HasComment("登录时间");
        b.Property(e => e.Mobile)
            .HasMaxLength(11)
            .HasDefaultValueSql("''")
            .HasComment("手机号码");
        b.Property(e => e.NickName)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("昵称");
        b.Property(e => e.PasswordHash)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("密码");
        b.Property(e => e.Salt)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("密码盐");
        b.Property(e => e.Status)
            .HasMaxLength(30)
            .HasDefaultValue(Status.normal)
            .HasComment("状态");
        b.Property(e => e.Token)
            .HasMaxLength(59)
            .HasDefaultValueSql("''")
            .HasComment("token标识");
    
        b.Property(e => e.UserName)
            .HasMaxLength(20)
            .HasDefaultValueSql("''")
            .HasComment("用户名");

        b.Property(e => e.CreateTime)
          .HasComment("创建时间");
        b.Property(e => e.UpdateTime)
            .HasComment("更新时间");
         
    }
}
