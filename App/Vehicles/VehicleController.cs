using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.VehicleMasterPage;
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
                Master = Url.Resource<VehicleMasterPageController>(),
                Data = new
                {
                    name = vehicle.Name,
                    year = vehicle.Year,
                    make = vehicle.MakeName,
                    model = vehicle.ModelName,
                    odometer = vehicle.Odometer,
                    photo = Url.Get<VehiclePhotoController>(new {id = vehicle.PhotoId}),
                    delete = Url.Delete<DeleteVehicleController>()
                }
            };
        }
    }
}