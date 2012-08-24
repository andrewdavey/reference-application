using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Server.ReferenceData;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle
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
            return new Page("Vehicles/Details")
            {
                Title = vehicle.Name,
                Master = Url.Resource<GetVehicleMasterPageController>(),
                Data = new
                {
                    name = vehicle.Name,
                    year = vehicle.Year,
                    make = vehicle.MakeName,
                    model = vehicle.ModelName,
                    odometer = vehicle.Odometer,
                    photo = Url.Get<GetVehiclePhotoController>(),
                    save = Url.Put<PutVehicleController>(),
                    delete = Url.Delete<DeleteVehicleController>(),
                    years = Url.Get<GetYearsController>()
                }
            };
        }
    }
}