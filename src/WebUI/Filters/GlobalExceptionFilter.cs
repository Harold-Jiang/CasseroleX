using CasseroleX.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUI.Filters;

/// <summary>
/// 全局异常过滤器
/// </summary>
public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IWebHostEnvironment _environment;
     
    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, 
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    { 
        string error = string.Empty;
        void ReadException(Exception ex)
        {
            error += $"{ex.Message} | {ex.StackTrace} | {ex.InnerException}";
            if (ex.InnerException != null)
            {
                ReadException(ex.InnerException);
            }
        }
        ReadException(context.Exception);

        _logger.LogError("异常信息：{error}",error);//记录错误日志

        Result apiResponse = new(0, _environment.IsDevelopment() ? error : "服务器异常，请稍后再试.", null, $"{context.HttpContext.Request.Path}", 3);
        HandlerException(context, apiResponse);
        return Task.CompletedTask;
    }

    private static void HandlerException(ExceptionContext context, Result apiResponse)
    {
        //根据请求方式返回异常
        if (HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            context.Result = new JsonResult(apiResponse)
            {
                StatusCode = StatusCodes.Status200OK, //post 返回200和JSON对象 前端根据JSON弹出警告窗
                ContentType = "application/json",
            };
        }
        context.ExceptionHandled = false;
    }
}
