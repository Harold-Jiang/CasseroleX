using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Configurations;
using CasseroleX.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Services;

public class UserManager : IUserManager 
{
    protected readonly IApplicationDbContext _context;
    protected readonly IEncryptionService _encryptionService;
    protected readonly ISiteConfigurationService _sysConfigService;

    public UserManager(IApplicationDbContext context,
        IEncryptionService encryptionService,
        ISiteConfigurationService sysConfigService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _sysConfigService = sysConfigService;
    }
    public virtual async Task<T?> GetUserByIdAsync<T>(int id, CancellationToken cancellationToken = default) where T : BaseUser
    {
        return  await _context.Set<T>().FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);
    }

    public virtual async Task<T?> GetUserByUserNameAsync<T>(string? userName, CancellationToken cancellationToken = default) where T : BaseUser
    {
        return string.IsNullOrWhiteSpace(userName)
            ? null
            : await _context.Set<T>().FirstOrDefaultAsync(x => x.UserName == userName.ToLowerInvariant(), cancellationToken);
    }

    public virtual async Task<T?> GetUserByEmailAsync<T>(string? email, CancellationToken cancellationToken = default) where T : BaseUser
    {
        return string.IsNullOrWhiteSpace(email)
            ? null
            : await _context.Set<T>().FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public virtual async Task<T?> GetUserByMobileAsync<T>(string? mobile, CancellationToken cancellationToken = default) where T : BaseUser
    {
        return string.IsNullOrWhiteSpace(mobile)
            ? null
            : await _context.Set<T>().FirstOrDefaultAsync(x => x.Mobile == mobile.ToLowerInvariant(), cancellationToken);
    }

    public virtual async Task<bool> UpdateAsync<T>(T entity, string[]? propertyNames = null, CancellationToken cancellationToken = default) where T : BaseUser
    { 
        if (propertyNames != null && propertyNames.Length > 0) //按指定字段修改
        {
            //_context.Set<T>().Entry(entity).State = EntityState.Unchanged;
            foreach (var name in propertyNames)
            {
                _context.Set<T>().Entry(entity).Property(name).IsModified = true;
            }
        }
        else
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Entry(entity).State = EntityState.Modified;
        }
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
   
}
