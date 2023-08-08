using CasseroleX.Application.Attachments.Commands.CreateAttachmentCommand;
using CasseroleX.Application.Attachments.Commands.DeleteAttachmentCommand;
using CasseroleX.Application.Attachments.Commands.UpdateAttachmentCommand;
using CasseroleX.Application.Attachments.Queries;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Configurations;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using CasseroleX.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.General;

[Route("general/attachment")]
public class AttachmentController:BaseAdminController
{
    private readonly ISiteConfigurationService _siteConfigurationService;

    public AttachmentController(ISiteConfigurationService siteConfigurationService)
    {
        _siteConfigurationService = siteConfigurationService;
    }

    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync(SearchQuery<Attachment, AttachmentDto> query)
    {
        if (IsAjaxRequest())
        {
            var result = await Mediator.Send(query);
            return Json(new { Total = result.TotalCount, Rows = result.Items });
        }
        var _upload = await _siteConfigurationService.GetConfigurationAsync<UploadConfigInfo>();
        ViewBag.JsConfig.Add("categoryList", _upload.AttachmentCategory); 
        return View(_upload);

    }


    [HttpPost("upload")] 
    public async Task<Result> UpLoad(IFormCollection formCollection)
    {
        if (formCollection == null || formCollection.Files == null)
            throw new ArgumentNullException("form");
        
        CreateAttachmentCommand command = new CreateAttachmentCommand()
        {
            UserId = User.Identity.ID(),
            Category = Request.Form["category"].ToString(),
            File = formCollection.Files[0]
        };

        return await Mediator.Send(command);
    }
  
    [HttpGet("edit/ids/{ids}")]
    [HttpPost("edit/ids/{ids}")]
    public async Task<IActionResult> EditAsync(string ids, UpdateAttachmentCommand command)
    {
        if (IsPost())
        {
            command.Id = ids.ToInt();
            return Json(await Mediator.Send(command));
        }
        var admin = await Mediator.Send(new GetAttachmentDetailQuery(ids.ToInt()));
        return View(admin);
    }

    [HttpPost("del")]
    public async Task<Result> DeleteAsync(DeleteAttachmentCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("icon")] 
    public IActionResult Icon(string suffix)
    {
        suffix = suffix.IsNotNullOrEmpty() ? suffix : "FILE";
        var data = ImageExtensions.BuildSuffixImage(suffix);
        var offset = 30 * 60 * 60 * 24; 
        Response.Headers.Append("Cache-Control", $"public, max-age={offset}");
        return Content(data, "image/svg+xml");
    }

}
