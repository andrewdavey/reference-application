using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class ReminderController : ApiController
    {
        readonly GetReminder getReminder;
        readonly GetVehicleById getVehicleById;
        readonly FulfillReminder fulfillReminder;

        public ReminderController(GetReminder getReminder, GetVehicleById getVehicleById, FulfillReminder fulfillReminder)
        {
            this.getReminder = getReminder;
            this.getVehicleById = getVehicleById;
            this.fulfillReminder = fulfillReminder;
        }

        public object GetReminder(int vehicleId, int id)
        {
            var reminder = getReminder.Execute(id);
            var vehicle = getVehicleById.Execute(1, vehicleId);
            return new ReminderResource(reminder, vehicle, Url);
        }

        public void PatchReminder(int id, ReminderUpdate update)
        {
            if (update.IsFulfilled)
            {
                fulfillReminder.Execute(1, id);
            }
        }
    }
}