using CasseroleX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CasseroleX.Infrastructure.Persistence.Configurations;
public class AdminLogConfiguration : IEntityTypeConfiguration<AdminLog>
{
    public void Configure(EntityTypeBuilder<AdminLog> entity)
    {
        entity.HasIndex(e => e.UserName);

        entity.Property(e => e.Id)
            .HasComment("ID");

        entity.Property(e => e.AdminId)
            .HasComment("管理员ID");

        entity.Property(e => e.Content)
            .IsRequired()
            .HasComment("内容");

        entity.Property(e => e.CreateTime)
            .HasDefaultValue(DateTime.Now)
            .HasComment("操作时间");

        entity.Property(e => e.Ip)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("IP");

        entity.Property(e => e.Title)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("日志标题");

        entity.Property(e => e.Url)
            .HasMaxLength(1500)
            .HasDefaultValueSql("''")
            .HasComment("操作页面");

        entity.Property(e => e.UserAgent)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("User-Agent");

        entity.Property(e => e.UserName)
            .HasMaxLength(30)
            .HasDefaultValueSql("''")
            .HasComment("管理员名字");
    }
}
