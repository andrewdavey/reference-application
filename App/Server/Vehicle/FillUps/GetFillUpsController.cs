using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle.FillUps
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
                Script = "Client/Vehicles/FillUps",
                Data = new
                {
                    fillUps,
                    add = Url.Post<GetFillUpsController>()
                }
            };
        }
    }
}