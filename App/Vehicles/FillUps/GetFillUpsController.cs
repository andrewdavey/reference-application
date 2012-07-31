using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.MasterPage;
using MileageStats.Domain.Handlers;

namespace App.Vehicles.FillUps
{
    public class GetFillUpsController : ApiController
    {
        readonly GetFillupsForVehicle getFillupsForVehicle;

        public GetFillUpsController(GetFillupsForVehicle getFillupsForVehicle)
        {
            this.getFillupsForVehicle = getFillupsForVehicle;
        }

        public object GetFillUps(int vehicleId)
        {
            var fillUps = getFillupsForVehicle.Execute(vehicleId);
            return new Page
            {
                Title = "Fill ups",
                Master = Url.Resource<GetVehicleMasterPageController>(),
                Data = new
                {
                    fillUps,
                    add = Url.Post<GetFillUpsController>()
                }
            };
        }
    }
}