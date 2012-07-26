using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class PostFillUpController : ApiController
    {
        readonly AddFillupToVehicle addFillupToVehicle;

        public PostFillUpController(AddFillupToVehicle addFillupToVehicle)
        {
            this.addFillupToVehicle = addFillupToVehicle;
        }

        public void PostFillUp(int id, NewFillUp fillUp)
        {
            addFillupToVehicle.Execute(1, id, fillUp);
        }
    }
}