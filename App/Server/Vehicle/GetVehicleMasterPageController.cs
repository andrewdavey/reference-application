using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;

namespace App.Server.Vehicle
{
    public class GetVehicleMasterPageController : ApiController
    {
        public object GetMasterPage()
        {
            return new Page("Vehicles/MasterPage")
            {
                Data = new
                {
                    vehicles = Url.Get<GetVehiclesController>()
                }
            };
        }
    }
}
