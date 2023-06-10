
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Login.Commands;
using CasseroleX.Infrastructure.Authentication;
using CasseroleX.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

/// <summary>
/// 账户控制器
/// </summary>

public class AccountController : BaseAdminController
{
    private readonly ICustomAuthenticationService _authenticationService;
    private readonly ICacheService _cache; 

    public AccountController(
        ICacheService cache,
        ICustomAuthenticationService authenticationService) 
    {
        _cache = cache;
        _authenticationService = authenticationService;
    }

    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous] 
    public IActionResult Login(string? returnUrl = null)
    {
        if (_authenticationService.IsSignedIn())
        {
            return View("Jump", Result.Failure("用户已经登陆，请勿重复操作！", Url.Action("Index", "Index")));
        }
        ViewData["ReturnUrl"] = returnUrl; 
        return View();
    }


    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<Result> Login(AdminLoginCommand model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        var url = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Index", "Index");

        var result = await Mediator.Send(model);
        result.Url = url;
        return result is not null ? Result.Success(result,"登陆成功") : Result.Failure("登陆失败");
    }


    /// <summary>
    /// 获取图形验证码
    /// 示例：/GetCode
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Captcha()
    {
        string code = VerificationCodeGenerate.RandomCode(4);
        var content = VerificationCodeGenerate.GetCaptcha(code, 100, 33);
        HttpContext.Session.SetString("CaptchaCode",code);
        return File(content, "image/png", content.Length.ToString());

    }

    [AcceptVerbs("Get", "Post")]
    public async Task<IActionResult> LogOff()
    {
        if (IsGet())
        {
            var html = "<form id='logout_submit' name='logout_submit' action='' method='post'><input type='submit' value='ok' style='display:none;'></form>";
            html += "<script>document.forms['logout_submit'].submit();</script>";
            return Content(html, "text/html");
        }
        await _authenticationService.SignOut();
        return RedirectToAction(nameof(AccountController.Login), "Account");
    }

}
