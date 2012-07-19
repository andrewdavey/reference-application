using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.ReferenceData;

namespace App.Vehicles
{
    public class GetNewVehicleController : ApiController
    {
        public object Get()
        {
            return new Page
            {
                Title = "New Vehicle",
                Script = "Vehicles/NewVehiclePage",
                Data = new
                {
                    years = new { get = Url.Resource<YearsController>() },
                    save = new { post = Url.Resource<VehiclesController>() }
                }
            };
        }
    }
}