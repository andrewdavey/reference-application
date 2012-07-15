using System.Web.Mvc;
using System.Web.Routing;
using App.Infrastructure.Web;
using App.Pages.Dashboard;
using App.Pages.Vehicle;
using App.Pages.Vehicles;

namespace App
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapResource<DashboardController>("");
            routes.MapResource<VehiclesController>("vehicles");
            routes.MapResource<VehicleController>("vehicles/{id}");

        }
    }
}