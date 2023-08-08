using Microsoft.AspNetCore.Mvc.ApplicationModels;
using WebUI.Controllers;

namespace WebUI.Services;

public class CustomRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        // Check if the controller inherits from BaseController
        if (typeof(BaseAdminController).IsAssignableFrom(controller.ControllerType))
        {
            foreach (var action in controller.Actions)
            {
                var hasOverrideAttribute = action.Attributes.Any(attr => attr is OverrideBaseActionAttribute);

                if (hasOverrideAttribute)
                {
                    var baseAction = controller.Actions.FirstOrDefault(a =>
                        a.ActionName == action.ActionName &&
                        a.Controller.ControllerType == typeof(BaseAdminController) &&
                        !a.Attributes.Any(attr => attr is OverrideBaseActionAttribute));

                    if (baseAction != null)
                    {
                        action.Selectors[0].AttributeRouteModel = baseAction.Selectors[0].AttributeRouteModel;
                    }
                }
            }
        }
    }
}
