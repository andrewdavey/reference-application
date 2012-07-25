using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;

namespace App.Vehicles.VehicleMasterPage
{
    public class VehicleMasterPageController : ApiController
    {
        public object GetMasterPage()
        {
            return new Page
            {
                Script = "Vehicles/VehicleMasterPage",
                Data = new
                {
                    vehicles = Url.Get<VehiclesController>()
                }
            };
        }
    }
}
