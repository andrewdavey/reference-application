using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.List;

namespace App.Vehicles.MasterPage
{
    public class GetVehicleMasterPageController : ApiController
    {
        public object GetMasterPage()
        {
            return new Page
            {
                Data = new
                {
                    vehicles = Url.Get<GetVehiclesController>()
                }
            };
        }
    }
}
