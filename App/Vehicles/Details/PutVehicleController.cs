using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles.Details
{
    public class PutVehicleController : ApiController
    {
        readonly UpdateVehicle updateVehicle;

        public PutVehicleController(UpdateVehicle updateVehicle)
        {
            this.updateVehicle = updateVehicle;
        }

        public void PutVehicle(int vehicleId, VehicleUpdate update)
        {
            update.VehicleId = vehicleId;
            updateVehicle.Execute(1, update, update.Photo);
        }
    }
}