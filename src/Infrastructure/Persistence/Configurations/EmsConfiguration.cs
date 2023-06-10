using CasseroleX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class EmsConfiguration : IEntityTypeConfiguration<Ems>
{
    public void Configure(EntityTypeBuilder<Ems> b)
    {
        b.HasKey(s => s.Id);

        b.Property(e => e.Id)
            .HasComment("ID");
        b.Property(e => e.Code)
            .HasMaxLength(10)
            .HasDefaultValueSql("''")
            .HasComment("验证码");
        b.Property(e => e.Event)
            .HasMaxLength(30)
            .HasDefaultValueSql("''")
            .HasComment("事件");
        b.Property(e => e.Ip)
            .HasMaxLength(30)
            .HasDefaultValueSql("''")
            .HasComment("IP");
        b.Property(e => e.Email)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasComment("邮箱");
        b.Property(e => e.Times)
            .HasComment("验证次数");
        b.Property(e => e.CreateTime)
            .HasComment("创建时间")
            .HasDefaultValue(DateTime.Now);
    }
}
