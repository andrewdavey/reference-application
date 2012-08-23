using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Contracts;
using MileageStats.Domain.Handlers;
using MileageStats.Domain.Models;

namespace App.Server.Vehicle.FillUps
{
    public class PostFillUpsController : ApiController
    {
        readonly CanAddFillup canAddFillup;
        readonly AddFillupToVehicle addFillupToVehicle;

        public PostFillUpsController(CanAddFillup canAddFillup, AddFillupToVehicle addFillupToVehicle)
        {
            this.canAddFillup = canAddFillup;
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

        public class NewFillUp : ICreateFillupEntryCommand
        {
            public int FillupEntryId { get; set; }
            public int VehicleId { get; set; }
            public DateTime? Date { get; set; }
            public int Odometer { get; set; }
            public double? PricePerUnit { get; set; }
            public double? TotalUnits { get; set; }
            public FillupUnits UnitOfMeasure { get; set; }
            public string Vendor { get; set; }
            public string Location { get; set; }
            public double? TransactionFee { get; set; }
            public string Remarks { get; set; }
            public int? Distance { get; set; }
        }
    }
}