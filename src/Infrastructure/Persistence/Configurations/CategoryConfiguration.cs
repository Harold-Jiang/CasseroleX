using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CasseroleX.Infrastructure.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        entity.HasIndex(e => e.Pid);

        entity.HasIndex(e => new { e.Weigh, e.Id });

        entity.Property(e => e.Id);

        entity.Property(e => e.CreateTime)
            .HasComment("创建时间");

        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("描述");

        entity.Property(e => e.DiyName)
            .HasMaxLength(30)
            .HasDefaultValueSql("''")
            .HasComment("自定义名称");

        entity.Property(e => e.Image)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("图片");

        entity.Property(e => e.Keywords)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("关键字");

        entity.Property(e => e.Name)
            .HasMaxLength(30)
            .HasDefaultValueSql("''");

        entity.Property(e => e.NickName)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        entity.Property(e => e.Pid)
            .HasComment("父ID");

        entity.Property(e => e.Status)
            .HasMaxLength(30)
            .HasDefaultValue(Status.normal)
            .HasComment("状态");

        entity.Property(e => e.Type)
            .HasMaxLength(30)
            .HasColumnName("type")
            .HasComment("栏目类型");

        entity.Property(e => e.UpdateTime)
            .HasComment("更新时间");

        entity.Property(e => e.Weigh)
            .HasComment("权重");
    }
}
