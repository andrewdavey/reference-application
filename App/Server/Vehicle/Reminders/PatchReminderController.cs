using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle.Reminders
{
    public class PatchReminderController : ApiController
    {
        readonly FulfillReminder fulfillReminder;

        public PatchReminderController(FulfillReminder fulfillReminder)
        {
            this.fulfillReminder = fulfillReminder;
        }

        public void PatchReminder(int reminderId, ReminderUpdate update)
        {
            if (update.IsFulfilled)
            {
                fulfillReminder.Execute(1, reminderId);
            }
        }

        public class ReminderUpdate
        {
            public bool IsFulfilled { get; set; }
        }
    }
}