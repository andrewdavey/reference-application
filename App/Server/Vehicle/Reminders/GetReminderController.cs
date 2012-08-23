using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle.Reminders
{
    public class GetReminderController : ApiController
    {
        readonly GetReminder getReminder;
        readonly GetVehicleById getVehicleById;

        public GetReminderController(GetReminder getReminder, GetVehicleById getVehicleById)
        {
            this.getReminder = getReminder;
            this.getVehicleById = getVehicleById;
        }

        public object GetReminder(int vehicleId, int reminderId)
        {
            var reminder = getReminder.Execute(reminderId);
            var vehicle = getVehicleById.Execute(1, vehicleId);
            return new ReminderResource(reminder, vehicle, Url);
        }

    }
}