using System.Web;
using System.Web.Http;
using MileageStats.Domain.Contracts;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
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

    public class VehicleUpdate : ICreateVehicleCommand
    {
        public int VehicleId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public HttpPostedFileBase Photo { get; set; }

        string ICreateVehicleCommand.MakeName
        {
            get { return Make; }
            set { Make = value; }
        }

        string ICreateVehicleCommand.ModelName
        {
            get { return Model; }
            set { Model = value; }
        }
    }
}