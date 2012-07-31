using System.Web.Mvc;
using System.Web.Routing;
using App.Infrastructure.Web;

namespace App
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Resource("Dashboard", "");
            routes.Resource("Vehicles", "vehicles");
            routes.Resource("NewVehicle", "vehicles/new");
            routes.Resource("VehicleMasterPage", "vehicles/master");
            routes.Resource("Vehicle", "vehicles/{vehicleId}");
            routes.Resource("VehiclePhoto", "vehicles/{id}/photo");
            routes.Resource("FillUps", "vehicles/{id}/fillUps");
            routes.Resource("Reminders", "vehicles/{id}/reminders");
            routes.Resource("Reminder", "vehicles/{vehicleId}/reminders/{id}");
            routes.Resource("Profile", "profile");
            routes.Resource("Years", "reference/years");
            routes.Resource("Makes", "reference/years/{year}");
            routes.Resource("Models", "reference/years/{year}/{make}");
            routes.Resource("Countries", "reference/countries");

            routes.Resource("Specs", "specs");
        }
    }
}