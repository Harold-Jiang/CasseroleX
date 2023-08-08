
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Login.Commands;
using CasseroleX.Application.Utils;
using CasseroleX.Infrastructure.Authentication;
using CasseroleX.Infrastructure.Common;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

/// <summary>
/// Account Login Logoff
/// </summary>

public class AccountController : BaseAdminController
{
    private readonly ICustomAuthenticationService _authenticationService;
    private readonly ICacheService _cache;
    private readonly IAntiforgery _antiforgery;
    public AccountController(
        ICacheService cache,
        ICustomAuthenticationService authenticationService,
        IAntiforgery antiforgery)
    {
        _cache = cache;
        _authenticationService = authenticationService;
        _antiforgery = antiforgery;
    }

    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous] 
    public IActionResult Login(string? returnUrl = null)
    {
        if (_authenticationService.IsSignedIn())
        {
            return View("Jump", Result.Failure(OperationResult.LOGGED_IN, Url.Action("Index", "Index")));
        }
        ViewData["ReturnUrl"] = returnUrl; 
        return View();
    }


    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    public async Task<Result> Login(AdminLoginCommand model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        var url = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Index", "Index");

        var result = await Mediator.Send(model);
        result.Url = url;
        return result is not null ? Result.Success(result,OperationResult.LOGIN_SUCCESS) : Result.Failure();
    }


    /// <summary>
    /// image captcha
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Captcha()
    {
        string code = RandomProvider.RandomCode(4);
        var content = VerificationCodeGenerate.GetCaptcha(code, 100, 33);
        HttpContext.Session.SetString("CaptchaCode",code);
        return File(content, "image/png", content.Length.ToString());

    }

    [AcceptVerbs("Get", "Post")]
    public async Task<IActionResult> LogOff()
    {
        if (IsPost())
        {
            await _authenticationService.SignOut();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        //create VerificationToken
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        var tokenValue = tokens.RequestToken;

        var html = "<form id='logout_submit' name='logout_submit' action='' method='post'>";
        html += "<input type='hidden' name='__RequestVerificationToken' value='" + tokenValue + "' />";
        html += "<input type='submit' value='ok' style='display:none;'></form>";
        html += "<script>document.getElementById('logout_submit').submit();</script>";
        return Content(html, "text/html");
    }

}
