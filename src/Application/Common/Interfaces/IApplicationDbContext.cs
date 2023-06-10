using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Entities.Role;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    DbSet<Admin> Admins { get; }

    DbSet<AdminRole> AdminRoles { get; }
    DbSet<Role> Roles { get; }
    DbSet<RolePermissions> RolePermissions { get; }
    DbSet<User> Users { get; }
    DbSet<UserGroup> UserGroups { get; }
    DbSet<Sms> Sms { get; }
    DbSet<Ems> Ems { get; }
    DbSet<Attachment> Attachment { get; }
    DbSet<SiteConfiguration> SiteConfigurations { get; }

    DbSet<Category> Categories { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
