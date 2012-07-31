using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.ReferenceData;
using App.Vehicles.VehicleMasterPage;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class GetVehicleController : ApiController
    {
        readonly GetVehicleById getVehicleById;

        public GetVehicleController(GetVehicleById getVehicleById)
        {
            this.getVehicleById = getVehicleById;
        }

        public object GetVehicle(int vehicleId)
        {
            var vehicle = getVehicleById.Execute(1, vehicleId);
            return new Page
            {
                Title = vehicle.Name,
                Script = "Vehicles/VehiclePage",
                Master = Url.Resource<GetVehicleMasterPageController>(),
                Data = new
                {
                    name = vehicle.Name,
                    year = vehicle.Year,
                    make = vehicle.MakeName,
                    model = vehicle.ModelName,
                    odometer = vehicle.Odometer,
                    photo = Url.Get<GetVehiclePhotoController>(new {id = vehicle.PhotoId}),
                    save = Url.Put<PutVehicleController>(),
                    delete = Url.Delete<DeleteVehicleController>(),
                    years = Url.Get<GetYearsController>()
                }
            };
        }
    }
}