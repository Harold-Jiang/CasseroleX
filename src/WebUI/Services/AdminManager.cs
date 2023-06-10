using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Configurations;
using CasseroleX.Application.Login.Commands;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Exceptions;
using CasseroleX.Infrastructure.Authentication;
using WebUI.Helpers;

namespace WebUI.Services;

public class AdminManager : UserManager,IAdminManager
{
    private readonly ICustomAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _contextAccessor;

    public AdminManager(IApplicationDbContext context,
        IEncryptionService encryptionService,
        ISiteConfigurationService sysConfigService,
        ICustomAuthenticationService authenticationService,
        IHttpContextAccessor contextAccessor) : base(context, encryptionService, sysConfigService)
    {
        _authenticationService = authenticationService;
        _contextAccessor = contextAccessor;
    }


    public async Task<AdminLoginResultDto> Login(AdminLoginCommand model,CancellationToken cancellationToken =default)
    {
        //获取配置项 
        var accountConfig = await _sysConfigService.GetConfiguration<AccountConfigInfo>();
        //检查验证码
        if (accountConfig is not null && accountConfig.LoginCaptcha)
        {
            var captcha  = _contextAccessor.HttpContext?.Session.GetString("CaptchaCode") ??"";
            if (!model.Code.IsNotNullOrEmpty() || model.Code.ToLower() != captcha.ToLower())
            {
                throw new ImageVerificationCodeException("验证码错误");
            }
            //验证完毕，删除验证码
            _contextAccessor.HttpContext?.Session.Remove("CaptchaCode");
        }

        //查找用户
        var admin = await GetUserByUserNameAsync<Admin>(model.UserName, cancellationToken);
        if (admin == null || !_encryptionService.ValidatePasswordHash(model.Password, admin.PasswordHash, admin.Salt))
        {
            throw new AccountException("用户不存在或密码错误");
        }

        //是否锁定
        if (admin.LockoutEnabled)
            throw new AccountException("用户账号已被禁用或锁定");

        await _authenticationService.SignIn(admin, model.RememberMe);

        //记录本次登录的IP和时间
        admin.LoginIp = WebTools.GetIpAddress(_contextAccessor.HttpContext);
        admin.LoginTime = DateTime.Now;
        //重置登录失败次数
        admin.LoginFailure = 0;

        bool updateResult = await UpdateAsync(admin,new string[]{ "LoginIp", "LoginTime", "LoginFailure" },cancellationToken);

        //_contextAccessor.HttpContext?.Session.SetString("admin", _mapper.Map<AdminDto>(admin).ToJson());

        return !updateResult
            ? throw new AccountException("登录失败")
            : new AdminLoginResultDto() { Id = admin.Id, UserName = admin.UserName!, Avatar = admin.Avatar };
     
    }
     
}
