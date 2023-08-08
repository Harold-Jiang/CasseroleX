using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Entities.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CasseroleX.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    //DatabaseFacade DataBase { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    DbSet<Admin> Admins { get; }
    DbSet<AdminLog> AdminLogs { get; }
    DbSet<AdminRole> AdminRoles { get; }
    DbSet<Role> Roles { get; }
    DbSet<RolePermissions> RolePermissions { get; }
    DbSet<User> Users { get; }
    DbSet<UserRule> UserRules { get; }
    DbSet<UserGroup> UserGroups { get; }
    DbSet<Sms> Sms { get; }
    DbSet<Ems> Ems { get; }
    DbSet<Attachment> Attachments { get; }
    DbSet<SiteConfiguration> SiteConfigurations { get; }

    DbSet<Category> Categories { get; }


    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IEntityType? GetEntityType(string tableName);

    List<string?> GetTableList();
}
