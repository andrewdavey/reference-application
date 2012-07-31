using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.List;
using App.Vehicles.ReferenceData;

namespace App.Vehicles.New
{
    public class GetNewVehicleController : ApiController
    {
        public object Get()
        {
            return new Page
            {
                Title = "New Vehicle",
                Script = "Vehicles/New",
                Data = new
                {
                    years = Url.Get<GetYearsController>(),
                    save = Url.Post<GetVehiclesController>()
                }
            };
        }
    }
}