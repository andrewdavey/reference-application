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
            return new Page
            {
                Title = "New Vehicle",
                Script = "Client/Vehicles/New",
                Data = new
                {
                    years = Url.Get<GetYearsController>(),
                    save = Url.Post<GetVehiclesController>()
                }
            };
        }
    }
}