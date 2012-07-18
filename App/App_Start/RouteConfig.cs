using System.Web.Mvc;
using System.Web.Routing;
using App.Infrastructure.Web;
using App.Modules.ReferenceData;
using App.Modules.Vehicles;
using App.Pages.GetNewVehicle;
using App.Pages.Dashboard;
using App.Pages.Vehicle;

namespace App
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapResource<DashboardController>("");
            routes.GetResource<VehiclesController>("vehicles");
            routes.PostResource<PostVehicleController>("vehicles");
            routes.MapResource<GetNewVehicleController>("vehicles/add");
            routes.MapResource<VehicleController>("vehicles/{id}");

            routes.MapResource<YearsController>("reference/years");
            routes.MapResource<MakesController>("reference/years/{year}");
            routes.MapResource<ModelsController>("reference/years/{year}/{make}");
        }
    }
}