using CasseroleX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class SysConfigConfiguration : IEntityTypeConfiguration<SiteConfiguration>
{
    public void Configure(EntityTypeBuilder<SiteConfiguration> b)
    {
        b.HasKey(s => s.Id);
        b.Property(t => t.Name).HasMaxLength(30).IsRequired();
        b.Property(t => t.Group).HasMaxLength(30).IsRequired();
        b.Property(t => t.Type).HasMaxLength(30);
        b.Property(t => t.Tip).HasMaxLength(100);
        b.Property(t => t.Title).HasMaxLength(100);
        b.Property(t => t.Rule).HasMaxLength(100);
        b.Property(t => t.Visible).HasMaxLength(255);
        b.Property(t => t.Extend).HasMaxLength(255);
    }
}
