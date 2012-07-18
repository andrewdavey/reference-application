using System.Web.Http;
using App.Infrastructure.Web;
using App.Modules.ReferenceData;
using App.Modules.Vehicles;

namespace App.Pages.GetNewVehicle
{
    public class GetNewVehicleController : ApiController
    {
        public object Get()
        {
            return new
            {
                years = new { get = Url.Resource<YearsController>() },
                save = new { post = Url.Resource<VehiclesController>() }
            };
        }
    }
}