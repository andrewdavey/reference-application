using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Pages.Vehicle
{
    public class VehicleController : ApiController
    {
        readonly GetVehicleById getVehicleById;

        public VehicleController(GetVehicleById getVehicleById)
        {
            this.getVehicleById = getVehicleById;
        }

        public object GetVehicle(int id)
        {
            return new
            {
                vehicle = getVehicleById.Execute(1, id)
            };
        }
    }
}