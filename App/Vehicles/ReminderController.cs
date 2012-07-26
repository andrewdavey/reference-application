using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class ReminderController : ApiController
    {
        readonly GetReminder getReminder;

        public ReminderController(GetReminder getReminder)
        {
            this.getReminder = getReminder;
        }

        public object GetReminder(int vehicleId, int id)
        {
            return getReminder.Execute(id);
        }
    }
}