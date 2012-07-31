using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.Infrastructure.Web;
using App.Vehicles.Details;
using MileageStats.Domain.Handlers;

namespace App.Vehicles.New
{
    public class PostVehiclesController : ApiController
    {
        readonly CreateVehicle createVehicle;

        public PostVehiclesController(CreateVehicle createVehicle)
        {
            this.createVehicle = createVehicle;
        }

        public HttpResponseMessage PostVehicle(NewVehicleForm form)
        {
            var vehicleId = createVehicle.Execute(1, form, null);
            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Headers =
                {
                    {"Location", Url.Resource<GetVehicleController>(new {vehicleId})}
                }
            };
        }
    }
}