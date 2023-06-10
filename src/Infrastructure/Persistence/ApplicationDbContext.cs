using System.Reflection;
using System.Text.Json;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<AdminRole> AdminRoles => Set<AdminRole>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermissions> RolePermissions => Set<RolePermissions>();

    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();

    public DbSet<Sms> Sms => Set<Sms>();
    public DbSet<Ems> Ems => Set<Ems>();
    public DbSet<Attachment> Attachment => Set<Attachment>();
    public DbSet<SiteConfiguration> SiteConfigurations => Set<SiteConfiguration>();

    public DbSet<Category> Categories => Set<Category>();
    protected override void OnModelCreating(ModelBuilder builder)
    {  
        //加载默认配置
        base.OnModelCreating(builder);
        //读取自定义配置
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //种子数据
        JsonConvertDataSeedList<RolePermissions>(builder);
        JsonConvertDataSeedList<Role>(builder);
        JsonConvertDataSeedList<Admin>(builder);
        JsonConvertDataSeedList<AdminRole>(builder);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
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
