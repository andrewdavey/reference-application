using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class AddFillUpController : ApiController
    {
        readonly AddFillupToVehicle addFillupToVehicle;

        public AddFillUpController(AddFillupToVehicle addFillupToVehicle)
        {
            this.addFillupToVehicle = addFillupToVehicle;
        }

        public void PostFillUp(int id, NewFillUp fillUp)
        {
            addFillupToVehicle.Execute(1, id, fillUp);
        }
    }
}