using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using WebUI.Helpers;

namespace WebUI.Services;

/// <summary>
/// 自定义视图返回对象
/// </summary>
public class CustomViewEngine : ViewResultExecutor
{
    public CustomViewEngine(IOptions<MvcViewOptions> viewOptions, IHttpResponseStreamWriterFactory writerFactory, ICompositeViewEngine viewEngine, ITempDataDictionaryFactory tempDataFactory, DiagnosticListener diagnosticListener, ILoggerFactory loggerFactory, IModelMetadataProvider modelMetadataProvider) : base(viewOptions, writerFactory, viewEngine, tempDataFactory, diagnosticListener, loggerFactory, modelMetadataProvider)
    {
    }

    public override ViewEngineResult FindView(ActionContext context, ViewResult viewResult)
    {

        var controllerName = WebTools.GetControllerName(context.HttpContext);
        var actionName = context.ActionDescriptor.RouteValues["action"];

        viewResult.ViewName = $"~/Views/{controllerName}/{actionName}.cshtml";

        return base.FindView(context, viewResult);
    }


}
