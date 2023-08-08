using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");

        b.HasKey(s => s.Id);

        b.HasIndex(e => e.Email);
        b.HasIndex(e => e.Mobile);
        b.HasIndex(e => e.UserName);

        b.Property(e => e.Id)
            .HasComment("ID");
        b.Property(e => e.Avatar)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("头像");
        b.Property(e => e.Bio)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("格言");
        b.Property(e => e.BirthDay)
            .HasComment("生日");
        b.Property(e => e.Email)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("电子邮箱");
        b.Property(e => e.Gender)
            .HasComment("性别");
        b.Property(e => e.GroupId)
            .HasComment("组别ID");
        b.Property(e => e.JoinIp)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("加入IP");
        b.Property(e => e.JoinTime)
            .HasComment("加入时间");
        b.Property(e => e.Level)
            .HasComment("等级");
        b.Property(e => e.LoginFailure)
            .HasComment("失败次数");
        b.Property(e => e.LoginIp)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("登录IP");
        b.Property(e => e.LoginTime)
            .HasComment("登录时间");
        b.Property(e => e.MaxSuccessions)
            .HasDefaultValueSql("'1'")
            .HasComment("最大连续登录天数");
        b.Property(e => e.Mobile)
            .HasMaxLength(11)
            .HasDefaultValueSql("''")
            .HasComment("手机号");
        b.Property(e => e.Money)
            .HasPrecision(10, 2)
            .HasComment("余额");
        b.Property(e => e.NickName)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("昵称");
        b.Property(e => e.PasswordHash)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("密码");
        b.Property(e => e.PrevTime)
            .HasComment("上次登录时间");
        b.Property(e => e.Salt)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("密码盐");
        b.Property(e => e.Score)
            .HasComment("积分");
        b.Property(e => e.Status)
            .HasMaxLength(30)
           .HasDefaultValue(Status.normal)
            .HasComment("状态");
        b.Property(e => e.Successions)
            .HasDefaultValueSql("'1'")
            .HasComment("连续登录天数");
        b.Property(e => e.Token)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("Token");
        
        b.Property(e => e.UserName)
            .HasMaxLength(32)
            .HasDefaultValueSql("''")
            .HasComment("用户名");

        b.OwnsOne(u => u.Verification, v =>
        {
            v.Property(p => p.Mobile).HasColumnName("MobileConfirm").HasDefaultValue(false).HasComment("手机号验证");
            v.Property(p => p.Email).HasColumnName("EmailConfirm").HasDefaultValue(false).HasComment("邮箱验证");
        }); 

    
        b.Property(e => e.CreateTime)
          .HasComment("创建时间");
        b.Property(e => e.UpdateTime)
            .HasComment("更新时间");


        b.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupId).IsRequired();
    }
}
