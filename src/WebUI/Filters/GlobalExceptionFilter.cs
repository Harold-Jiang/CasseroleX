using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUI.Filters;

/// <summary>
/// Global Exception Filter
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
        //Model validation exceptions directly return exception information
        if (context.Exception is ValidatorException validatorException)
        {
            List<string> errorList = validatorException.Errors.Values.SelectMany(arr => arr).ToList();
            HandlerException(context, new(0, string.Join("|", errorList), null, $"{context.HttpContext.Request.Path}", 3));
            return Task.CompletedTask;
        }
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

        //exception log
        _logger.LogError("CasseroleX Exception：{error}", error);

        Result apiResponse = new(0, _environment.IsDevelopment() ? error : "The server is abnormal. Please try again later.", null, $"{context.HttpContext.Request.Path}", 3);
        HandlerException(context, apiResponse);
        return Task.CompletedTask;
    }

    private static void HandlerException(ExceptionContext context, Result apiResponse)
    { 
        if (HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            context.Result = new JsonResult(apiResponse)
            {
                StatusCode = StatusCodes.Status200OK,  
                ContentType = "application/json",
            };
        }
        context.ExceptionHandled = false;
    }
}
