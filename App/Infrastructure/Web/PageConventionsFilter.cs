using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace App.Infrastructure.Web
{
    public class PageConventionsFilter : PageFilterBase
    {
        protected override void Execute(Page page, HttpActionContext actionContext, HttpResponseMessage response)
        {
            if (page.Script == null)
            {
                page.Script = ConventionalModulePath(actionContext);
            }
            if (page.Stylesheet == null)
            {
                page.Stylesheet = ConventionalModulePath(actionContext);
            }
        }

        string ConventionalModulePath(HttpActionContext actionContext)
        {
            // Example:
            // App.Vehicles.Details.GetVehicleController --> "Vehicles/Details"

            var controllerType = actionContext.ControllerContext.Controller.GetType();
            var names = controllerType
                .Namespace
                .Split('.')
                .Skip(1);
            return string.Join("/", names);
        }
    }
}