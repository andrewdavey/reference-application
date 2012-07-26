using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class PatchReminderController : ApiController
    {
        readonly FulfillReminder fulfillReminder;

        public PatchReminderController(FulfillReminder fulfillReminder)
        {
            this.fulfillReminder = fulfillReminder;
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