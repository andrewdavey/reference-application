using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.VehicleMasterPage;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class FillUpsController : ApiController
    {
        readonly GetFillupsForVehicle getFillupsForVehicle;

        public FillUpsController(GetFillupsForVehicle getFillupsForVehicle)
        {
            this.getFillupsForVehicle = getFillupsForVehicle;
        }

        public object GetFillUps(int id)
        {
            var fillUps = getFillupsForVehicle.Execute(id);
            return new Page
            {
                Title = "Fill ups",
                Script = "Vehicles/FillUpsPage",
                Stylesheet = "Vehicles/FillUpsPage",
                Master = Url.Resource<VehicleMasterPageController>(),
                Data = new
                {
                    fillUps,
                    add = Url.Post<FillUpsController>()
                }
            };
        }
    }
}