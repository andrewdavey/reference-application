using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
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

        public object GetReminder(int vehicleId, int id)
        {
            var reminder = getReminder.Execute(id);
            var vehicle = getVehicleById.Execute(1, vehicleId);
            return new ReminderResource(reminder, vehicle, Url);
        }

    }
}