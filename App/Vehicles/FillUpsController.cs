using System.Web.Http;
using App.Infrastructure;
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
                Data = new
                {
                    fillUps
                }
            };
        }
    }
}