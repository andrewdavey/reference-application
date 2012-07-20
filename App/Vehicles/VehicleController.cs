using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
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
            var vehicle = getVehicleById.Execute(1, id);
            return new Page
            {
                Title = vehicle.Name,
                Script = "Vehicles/VehiclePage",
                Data = new
                {
                    name = vehicle.Name,
                    year = vehicle.Year,
                    make = vehicle.MakeName,
                    model = vehicle.ModelName,
                    odometer = vehicle.Odometer,
                    photo = Url.Resource<VehiclePhotoController>(new {id = vehicle.PhotoId})
                }
            };
        }
    }
}