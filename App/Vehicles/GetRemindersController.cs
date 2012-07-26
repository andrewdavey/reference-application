using System.Linq;
using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.VehicleMasterPage;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
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

        public object GetReminders(int id)
        {
            var reminders = getAllRemindersForVehicle.Execute(id).Where(r => !r.IsFulfilled);
            
            return new Page
            {
                Master = Url.Resource<GetVehicleMasterPageController>(),
                Script = "Vehicles/Reminders",
                Stylesheet = "Vehicles/Reminders",
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
