using System.Web.Mvc;
using System.Web.Routing;
using App.Dashboard;
using App.Infrastructure.Web;
using App.Profile;
using App.Specs;
using App.Vehicles;
using App.Vehicles.ReferenceData;

namespace App
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapResource<DashboardController>("");
            routes.GetResource<VehiclesController>("vehicles");
            routes.PostResource<PostVehicleController>("vehicles");
            routes.MapResource<GetNewVehicleController>("vehicles/add");
            routes.MapResource<VehicleController>("vehicles/{id}");
            routes.MapResource<FillUpsController>("vehicles/{id}/fillups");
            routes.MapResource<VehiclePhotoController>("vehicles-photo/{id}");

            routes.MapResource<ProfileController>("profile");

            routes.MapResource<YearsController>("reference/years");
            routes.MapResource<MakesController>("reference/years/{year}");
            routes.MapResource<ModelsController>("reference/years/{year}/{make}");

            routes.MapResource<CountriesController>("reference/countries");

            routes.MapResource<SpecController>("specs");
        }
    }
}