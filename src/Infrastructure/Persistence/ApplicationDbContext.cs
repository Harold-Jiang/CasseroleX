using System.Reflection;
using System.Text.Json;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Domain.Common;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CasseroleX.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    //public DatabaseFacade DataBase => Database;

    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<AdminLog> AdminLogs => Set<AdminLog>();
    public DbSet<AdminRole> AdminRoles => Set<AdminRole>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermissions> RolePermissions => Set<RolePermissions>();

    public DbSet<User> Users => Set<User>();
    public DbSet<UserRule> UserRules => Set<UserRule>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();

    public DbSet<Sms> Sms => Set<Sms>();
    public DbSet<Ems> Ems => Set<Ems>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<SiteConfiguration> SiteConfigurations => Set<SiteConfiguration>();

    public DbSet<Category> Categories => Set<Category>();
    protected override void OnModelCreating(ModelBuilder builder)
    {   
        base.OnModelCreating(builder); 
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //DataSeed
        JsonConvertDataSeedList<RolePermissions>(builder);
        JsonConvertDataSeedList<Role>(builder);
        JsonConvertDataSeedList<Admin>(builder);
        JsonConvertDataSeedList<AdminRole>(builder);
        JsonConvertDataSeedList<SiteConfiguration>(builder);
        JsonConvertDataSeedList<User>(builder);
        JsonConvertDataSeedList<UserGroup>(builder);
        JsonConvertDataSeedList<UserRule>(builder);
         
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }


    public string GetPrimaryKeyName(IEntityType entityType)
    { 
        var primaryKey = entityType.FindPrimaryKey();
        if (primaryKey == null)
        {
            return "Id";  
        }
        return primaryKey.Properties[0].Name;
    }
    public IQueryable<dynamic>? GetEntitiesFromTableName(IEntityType entityType)
    {
        var method = typeof(DbContext).GetMethod(nameof(this.Set));
        var genericMethod = method?.MakeGenericMethod(entityType.ClrType);
        var dbSet = genericMethod?.Invoke(this, null);

        return dbSet is null? null: (IQueryable<BaseEntity>)dbSet;
    }

    public List<string?> GetTableList()
    {
        return base.Model.GetEntityTypes().Select(e => e.GetTableName()).ToList();
    }
    public IEntityType? GetEntityType(string tableName)
    {
        return base.Model.FindEntityType(tableName);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this); 
        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.RollbackTransactionAsync(cancellationToken);
    }

    private static void JsonConvertDataSeedList<T>(ModelBuilder modelBuilder) where T : class
    {
        IList<T>? list = JsonSerializer.Deserialize<IList<T>>(File.ReadAllText(GetCurrPath(@"/DataSeed/" + typeof(T).Name + ".json")));
        modelBuilder.Entity<T>().HasData(list!);
    }

    private static string GetCurrPath(string fileName)
    {
        return Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + fileName);
    }

}
