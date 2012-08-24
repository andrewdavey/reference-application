using System.Linq;
using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle.Reminders
{
    public class GetRemindersController : ApiController
    {
        readonly GetAllRemindersForVehicle getAllRemindersForVehicle;
        readonly GetVehicleById getVehicleById;

        public GetRemindersController(GetAllRemindersForVehicle getAllRemindersForVehicle, GetVehicleById getVehicleById)
        {
            this.getAllRemindersForVehicle = getAllRemindersForVehicle;
            this.getVehicleById = getVehicleById;
        }

        public object GetReminders(int vehicleId)
        {
            var reminders = getAllRemindersForVehicle.Execute(vehicleId).Where(r => !r.IsFulfilled);

            return new Page("Vehicles/Reminders")
            {
                Title = "Reminders",
                MasterPage = Url.Resource<GetVehicleMasterPageController>(),
                Data = new
                {
                    // TODO: Seems like a nasty SELECT N+1 bug here!
                    reminders = reminders.Select(r => new ReminderResource(r, getVehicleById.Execute(1, r.VehicleId), Url)),
                    add = Url.Post<PostRemindersController>()
                }
            };
        }
    }
}
