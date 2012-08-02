using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using App.Vehicles.Details;
using App.Vehicles.FillUps;
using App.Vehicles.Reminders;
using MileageStats.Domain.Handlers;
using MileageStats.Domain.Models;

namespace App.Vehicles.List
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
                details = Url.Get<GetVehicleController>(new { vehicle.VehicleId }),
                fillUps = Url.Get<GetFillUpsController>(new { vehicle.VehicleId }),
                reminders = Url.Get<GetRemindersController>(new { vehicle.VehicleId }),
                photo = Url.Get<GetVehiclePhotoController>(new { vehicle.VehicleId }),
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