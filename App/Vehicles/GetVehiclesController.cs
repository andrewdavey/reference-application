using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;
using MileageStats.Domain.Models;

namespace App.Vehicles
{
    public class GetVehiclesController : ApiController
    {
        readonly GetVehicleListForUser getVehicleListForUser;

        public GetVehiclesController(GetVehicleListForUser getVehicleListForUser)
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
                details = Url.Get<GetVehicleController>(new { id = vehicle.VehicleId }),
                fillUps = Url.Get<GetFillUpsController>(new { id = vehicle.VehicleId }),
                reminders = Url.Get<GetRemindersController>(new { id = vehicle.VehicleId }),
                photo = Url.Get<GetVehiclePhotoController>(new { id = vehicle.PhotoId }), // TODO: get photo url
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