using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace WebUI.Services;

/// <summary>
/// 自定义视图返回对象
/// </summary>
public class CustomViewEngine :  ViewResultExecutor
{
    public CustomViewEngine(IOptions<MvcViewOptions> viewOptions, IHttpResponseStreamWriterFactory writerFactory, ICompositeViewEngine viewEngine, ITempDataDictionaryFactory tempDataFactory, DiagnosticListener diagnosticListener, ILoggerFactory loggerFactory, IModelMetadataProvider modelMetadataProvider) : base(viewOptions, writerFactory, viewEngine, tempDataFactory, diagnosticListener, loggerFactory, modelMetadataProvider)
    {
    }

    public override ViewEngineResult FindView(ActionContext context, ViewResult viewResult)
    {
        var route = context.ActionDescriptor.AttributeRouteInfo;
        if (route != null && !string.IsNullOrEmpty(route.Template))
        {
            //根据[Route("Auth/Group")] 直接返回对应路径的视图页 
            var actionDescriptor = context.ActionDescriptor;
            var actionName = actionDescriptor.RouteValues["action"];
            var viewPath = $"~/Views/{route.Template}/{actionName}.cshtml";
            viewResult.ViewName = viewPath;
        }

        return base.FindView(context, viewResult);
    }

   
}
