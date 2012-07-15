using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using App.Pages.Vehicle;
using MileageStats.Domain.Handlers;

namespace App.Modules.Vehicles
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
                vehicles = _getVehicleListForUser.Execute(1).Select(v => new
                {
                    name = v.Name,
                    href = Url.Resource<VehicleController>(new {id = v.VehicleId})
                })
            };
        }
    }
}