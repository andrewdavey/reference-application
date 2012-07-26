using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class PostVehicleController : ApiController
    {
        readonly CreateVehicle createVehicle;

        public PostVehicleController(CreateVehicle createVehicle)
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
                    {"Location", Url.Resource<GetVehicleController>(new {id = vehicleId})}
                }
            };
        }
    }
}