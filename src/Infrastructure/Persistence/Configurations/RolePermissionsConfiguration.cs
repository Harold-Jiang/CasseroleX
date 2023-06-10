using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class RolePermissionsConfiguration : IEntityTypeConfiguration<RolePermissions>
{
    public void Configure(EntityTypeBuilder<RolePermissions> b)
    {
        b.HasKey(e => e.Id);
         
        b.HasIndex(e => e.Name).IsUnique();
        b.HasIndex(e => e.Pid);
        b.HasIndex(e => e.Weigh);
        b.Property(e => e.Id);
        b.Property(e => e.Condition)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("条件");
        b.Property(e => e.CreateTime)
            .HasComment("创建时间");
        b.Property(e => e.Extend)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("扩展属性");
        b.Property(e => e.Icon)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("图标");
        b.Property(e => e.IsMenu)
            .HasComment("是否为菜单");
        b.Property(e => e.MenuType)
            .HasComment("菜单类型")
            .HasColumnType("enum('addtabs','blank','dialog','ajax')");
        b.Property(e => e.Name)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("规则名称");
        b.Property(e => e.Pid)
            .HasComment("父ID");
        b.Property(e => e.PinYin)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("拼音");
        b.Property(e => e.Py)
            .HasMaxLength(30)
            .HasDefaultValueSql("''")
            .HasComment("拼音首字母");
        b.Property(e => e.Remark)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("备注");
        b.Property(e => e.Status)
            .HasMaxLength(30)
            .HasDefaultValue(Status.normal)
            .HasComment("状态");
        b.Property(e => e.Title)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("规则名称");
        b.Property(e => e.Type)
            .HasDefaultValueSql("'file'")
            .HasComment("menu为菜单,file为权限节点")
            .HasColumnType("enum('menu','file')");
        b.Property(e => e.LastModified)
            .HasComment("更新时间");
        b.Property(e => e.Url)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("规则URL");
        b.Property(e => e.Weigh)
            .HasComment("权重");
    }
}
