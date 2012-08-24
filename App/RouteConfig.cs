using System.Web.Mvc;
using System.Web.Routing;
using App.Infrastructure.Web;

namespace App
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            new RouteConfig(routes).RegisterRoutes();
        }

        readonly RouteCollection routes;

        RouteConfig(RouteCollection routes)
        {
            this.routes = routes;
        }

        void RegisterRoutes()
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            AppFrame();
            Dashboard();
            Vehicles();
            FillUps();
            Reminders();
            Profile();
            ReferenceData();
            Specs();
        }

        void AppFrame()
        {
            routes.Resource("AppFrame", "appframe");
        }

        void Dashboard()
        {
            routes.Resource("Dashboard", "");
        }

        void Vehicles()
        {
            routes.Resource("Vehicles", "vehicles");
            routes.Resource("NewVehicle", "vehicles/new");
            routes.Resource("VehicleMasterPage", "vehicles/master");
            routes.Resource("Vehicle", "vehicles/{vehicleId}");
            routes.Resource("VehiclePhoto", "vehicles/{vehicleId}/photo");
        }

        void FillUps()
        {
            routes.Resource("FillUps", "vehicles/{vehicleId}/fillUps");
        }

        void Reminders()
        {
            routes.Resource("Reminders", "vehicles/{vehicleId}/reminders");
            routes.Resource("Reminder", "vehicles/{vehicleId}/reminders/{reminderId}");
        }

        void Profile()
        {
            routes.Resource("Profile", "profile");
        }

        void ReferenceData()
        {
            routes.Resource("Years", "reference/years");
            routes.Resource("Makes", "reference/years/{year}");
            routes.Resource("Models", "reference/years/{year}/{make}");
            routes.Resource("Countries", "reference/countries");
        }

        void Specs()
        {
            routes.Resource("Specs", "specs");
        }
    }
}