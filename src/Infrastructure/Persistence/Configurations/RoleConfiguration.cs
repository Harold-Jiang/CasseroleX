using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> b)
    {
        b.ToTable("Roles");

        b.HasKey(s => s.Id);   
      
        b.Property(e => e.Name)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("组名");
        b.Property(e => e.Pid)
            .HasComment("父组别");
        b.Property(e => e.Rules)
            .HasComment("规则ID")
            .HasColumnType("text");
        b.Property(e => e.Status)
            .HasMaxLength(30)
            .HasDefaultValue(Status.normal)
            .HasComment("状态");

        b.Property(e => e.CreateTime)
          .HasComment("创建时间");
        b.Property(e => e.UpdateTime)
            .HasComment("更新时间");
         
    }
}
