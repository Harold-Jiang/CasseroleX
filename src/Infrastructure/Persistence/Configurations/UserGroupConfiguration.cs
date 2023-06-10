using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> b)
    {
        b.ToTable("UserGroups", tb => tb.HasComment("会员组表"));
        b.HasKey(e => e.Id).HasName("PRIMARY");

        b.Property(e => e.Id)
            .HasComment("ID");
        b.Property(e => e.Name)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("组名");
        b.Property(e => e.Rules)
            .HasComment("权限节点")
            .HasColumnType("text");
        b.Property(e => e.Status)
            .HasMaxLength(30)
          .HasDefaultValue(Status.normal)
            .HasComment("状态");

        b.Property(e => e.CreateTime)
          .HasComment("创建时间");
        b.Property(e => e.LastModified)
            .HasComment("更新时间");
         
    }
}
