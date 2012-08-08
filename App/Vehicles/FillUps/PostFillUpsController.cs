using System.Net;
using System.Net.Http;
using System.Web.Http;
using MileageStats.Domain.Handlers;
using App.Infrastructure.Web;

namespace App.Vehicles.FillUps
{
    public class PostFillUpsController : ApiController
    {
        readonly CanAddFillup canAddFillup;
        readonly AddFillupToVehicle addFillupToVehicle;

        public PostFillUpsController(CanAddFillup canAddFillup, AddFillupToVehicle addFillupToVehicle)
        {
            this.addFillupToVehicle = addFillupToVehicle;
        }

        public HttpResponseMessage PostFillUp(int vehicleId, NewFillUp fillUp)
        {
            var errors = canAddFillup.Execute(1, vehicleId, fillUp);
            ModelState.AddModelErrors(errors);

            if (ModelState.IsValid)
            {
                addFillupToVehicle.Execute(1, vehicleId, fillUp);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}