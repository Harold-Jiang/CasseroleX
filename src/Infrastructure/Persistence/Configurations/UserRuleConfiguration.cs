using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class UserRuleConfiguration : IEntityTypeConfiguration<UserRule>
{
    public void Configure(EntityTypeBuilder<UserRule> b)
    {
        b.ToTable("UserRules");

        b.HasKey(s => s.Id);

        b.Property(e => e.Id); 
        b.Property(e => e.Name)
          .HasMaxLength(100)
          .HasDefaultValueSql("''")
          .HasComment("组名");
        b.Property(e => e.Title)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("规则名称");
        b.Property(e => e.Pid)
            .HasComment("父组别");
        b.Property(e => e.IsMenu)
            .HasComment("是否为菜单");
        b.Property(e => e.Remark)
           .HasMaxLength(255)
           .HasDefaultValueSql("''")
           .HasComment("备注");
        b.Property(e => e.Status)
            .HasMaxLength(30)
            .HasDefaultValue(Status.normal)
            .HasComment("状态");
        b.Property(e => e.Weigh)
            .HasComment("权重");

        b.Property(e => e.CreateTime)
          .HasComment("创建时间");
        b.Property(e => e.UpdateTime)
            .HasComment("更新时间"); 
    }
}
