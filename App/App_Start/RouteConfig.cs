using System.Web.Mvc;
using System.Web.Routing;
using App.Dashboard;
using App.Infrastructure;
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

        }
    }
}