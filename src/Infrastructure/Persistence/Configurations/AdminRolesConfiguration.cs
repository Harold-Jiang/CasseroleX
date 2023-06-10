using CasseroleX.Domain.Entities.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class AdminRolesConfiguration : IEntityTypeConfiguration<AdminRole>
{
    public void Configure(EntityTypeBuilder<AdminRole> b)
    {
        b.ToTable("AdminRoles");
        b.HasKey(t => new {t.AdminId , t.RoleId });
        b.HasIndex(e => e.RoleId);
        b.HasIndex(e => e.AdminId);
        b.HasIndex(e => new { e.AdminId, e.RoleId }).IsUnique();

        b.Property(e => e.RoleId)
            .HasComment("级别ID");
        b.Property(e => e.AdminId)
            .HasComment("会员ID");

        b.HasOne(e => e.Role).WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
        b.HasOne(e => e.Admin).WithMany().HasForeignKey(ur => ur.AdminId).IsRequired();

    }
}
