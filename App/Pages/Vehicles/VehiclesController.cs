using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Pages.Vehicles
{
    public class VehiclesController : ApiController
    {
        private readonly GetVehicleListForUser _getVehicleListForUser;

        public VehiclesController(GetVehicleListForUser getVehicleListForUser)
        {
            _getVehicleListForUser = getVehicleListForUser;
        }

        public object GetVehicles()
        {
            return new
            {
                vehicles = _getVehicleListForUser.Execute(1)
            };
        }
    }
}