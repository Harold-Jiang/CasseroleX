using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Configurations.Commands.CheckFieldName;
using CasseroleX.Application.Configurations.Commands.CreateConfigCommand;
using CasseroleX.Application.Configurations.Commands.DeleteConfigCommand;
using CasseroleX.Application.Configurations.Commands.SendTestEmail;
using CasseroleX.Application.Configurations.Commands.UpdateConfigCommand;
using CasseroleX.Application.Configurations.Queries;
using CasseroleX.Infrastructure.OptionSetup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebUI.Controllers.General;

[Route("general/config")]
public class ConfigController : BaseAdminController
{
    private readonly AppOptions _app;

    public ConfigController(IOptionsSnapshot<AppOptions> app)
    {
        _app = app.Value;
    }

    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync()
    {
        var result = await Mediator.Send(new GetConfigsQuery()); 
        return View(result); 
    }
     
    [HttpPost("add")]
    public async Task<Result> AddAsync(CreateConfigCommand command)
    {
        return await Mediator.Send(command);
    }
     
    [HttpPost("edit")]
    public async Task<IActionResult> EditAsync(string ids)
    {
        return Json(await Mediator.Send(new UpdateConfigCommand { Form = Request.Form }));
    }

    [HttpPost("del")]
    public async Task<Result> DeleteAsync()
    {
        return await Mediator.Send(new DeleteConfigCommand(Request.Form["name"].ToString()));
    }


    [HttpPost("emailtest")]
    public async Task<Result> EmailTestAsync()
    {
        var receiver = Request.Form["receiver"].ToString();
        return await Mediator.Send(new SendTestEmailCommand(receiver));
    }

    [HttpPost("check")] 
    public async Task<Result> Check(string name)
    {
        if (!_app.AppDebug)
            return Result.Failure(OperationResult.NOT_DEVELOPMENT);

        return await Mediator.Send(new CheckFieldNameCommand(name));
    }
}
