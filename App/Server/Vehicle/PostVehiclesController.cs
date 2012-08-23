using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Contracts;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle
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

        public class NewVehicleForm : ICreateVehicleCommand
        {
            public int VehicleId { get; set; }
            public string Name { get; set; }
            public int SortOrder { get; set; }
            public int? Year { get; set; }
            public string MakeName { get; set; }
            public string ModelName { get; set; }
        }
    }
}