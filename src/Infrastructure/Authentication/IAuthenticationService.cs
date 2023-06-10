using CasseroleX.Domain.Common;

namespace CasseroleX.Infrastructure.Authentication;

public interface ICustomAuthenticationService
{
    bool IsSignedIn();

    Task SignIn<T>(T user, bool createPersistentCookie) where T : BaseUser;

    Task SignOut();

    Task<T?> GetAuthenticatedUser<T>() where T : BaseUser;

    Task<string> GetUserByCookies();

    Task SetCookieUser(int userId);

  
}
