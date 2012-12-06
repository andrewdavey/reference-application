using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Server.ReferenceData;

namespace App.Server.Vehicle
{
    public class GetNewVehicleController : ApiController
    {
        public object Get()
        {
            return new Page("Vehicles/New/init")
            {
                Title = "New Vehicle",
                Data = new
                {
                    years = Url.Get<GetYearsController>(),
                    save = Url.Post<GetVehiclesController>()
                }
            };
        }
    }
}