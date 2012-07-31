using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class DeleteVehicleController : ApiController
    {
        readonly DeleteVehicle deleteVehicle;

        public DeleteVehicleController(DeleteVehicle deleteVehicle)
        {
            this.deleteVehicle = deleteVehicle;
        }

        public void DeleteVehicle(int vehicleId)
        {
            deleteVehicle.Execute(1, vehicleId);
        }
    }
}