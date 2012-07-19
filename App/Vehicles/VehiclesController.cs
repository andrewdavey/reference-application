using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;
using MileageStats.Domain.Models;

namespace App.Vehicles
{
    public class VehiclesController : ApiController
    {
        readonly GetVehicleListForUser getVehicleListForUser;

        public VehiclesController(GetVehicleListForUser getVehicleListForUser)
        {
            this.getVehicleListForUser = getVehicleListForUser;
        }

        public object GetVehicles()
        {
            var vehicles = getVehicleListForUser.Execute(1).Select(VehicleSummary);
            return new {vehicles};
        }

        object VehicleSummary(VehicleModel vehicle)
        {
            return new
            {
                details = Url.Resource<VehicleController>(new { id = vehicle.VehicleId }),
                fillUps = Url.Resource<VehicleController>(new { id = vehicle.VehicleId }),
                reminders = Url.Resource<VehicleController>(new { id = vehicle.VehicleId }),
                photo = Url.Resource<VehiclePhotoController>(new{ id = vehicle.PhotoId }), // TODO: get photo url
                name = vehicle.Name,
                year = vehicle.Year,
                make = vehicle.MakeName,
                model = vehicle.ModelName,
                averageFuelEfficiency = vehicle.Statistics.AverageFuelEfficiency,
                averageCostToDrive = vehicle.Statistics.AverageCostToDrive,
                averageCostPerMonth = vehicle.Statistics.AverageCostPerMonth
            };
        }
    }
}