using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using App.Pages.Vehicle;
using MileageStats.Domain.Handlers;

namespace App.Modules.Vehicles
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
            return new
            {
                vehicles = getVehicleListForUser.Execute(1).Select(v => new
                {
                    details = Url.Resource<VehicleController>(new { id = v.VehicleId }),
                    fillUps = Url.Resource<VehicleController>(new { id = v.VehicleId }),
                    reminders = Url.Resource<VehicleController>(new { id = v.VehicleId }),
                    photo = "/todo", // TODO: get photo url
                    name = v.Name,
                    year = v.Year,
                    make = v.MakeName,
                    model = v.ModelName,
                    averageFuelEfficiency = v.Statistics.AverageFuelEfficiency,
                    averageCostToDrive = v.Statistics.AverageCostToDrive,
                    averageCostPerMonth = v.Statistics.AverageCostPerMonth
                })
            };
        }
    }
}