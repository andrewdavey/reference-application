using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Contracts;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle.Reminders
{
    public class PostRemindersController : ApiController
    {
        readonly AddReminderToVehicle addReminderToVehicle;
        readonly CanAddReminder canAddReminder;

        public PostRemindersController(CanAddReminder canAddReminder, AddReminderToVehicle addReminderToVehicle)
        {
            this.addReminderToVehicle = addReminderToVehicle;
            this.canAddReminder = canAddReminder;
        }

        public HttpResponseMessage PostReminder(int vehicleId, NewReminder reminder)
        {
            reminder.VehicleId = vehicleId;

            var errors = canAddReminder.Execute(1, reminder);
            ModelState.AddModelErrors(errors);

            if (ModelState.IsValid)
            {
                addReminderToVehicle.Execute(1, vehicleId, reminder);
                return ReminderCreated(reminder);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        HttpResponseMessage ReminderCreated(NewReminder reminder)
        {
            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Headers =
                {
                    Location = ReminderUrl(reminder)
                }
            };
        }

        Uri ReminderUrl(NewReminder reminder)
        {
            var url = Url.Resource<GetReminderController>(new
            {
                reminder.VehicleId,
                reminder.ReminderId
            });
            return new Uri(url, UriKind.Relative);
        }
    }

    public class NewReminder : ICreateReminderCommand
    {
        public int ReminderId { get; set; }
        public int VehicleId { get; set; }
        public string Title { get; set; }
        public string Remarks { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DueDistance { get; set; }
        public bool IsFulfilled { get; set; }
    }
}