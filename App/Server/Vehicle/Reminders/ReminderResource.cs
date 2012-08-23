using System;
using System.Web.Http.Routing;
using App.Infrastructure.Web;
using MileageStats.Domain.Models;

namespace App.Server.Vehicle.Reminders
{
    public class ReminderResource
    {
        public ReminderResource(Reminder reminder, VehicleModel vehicle, UrlHelper url)
        {
            reminder.CalculateIsOverdue(vehicle.Odometer ?? 0);

            Title = reminder.Title;
            Remarks = reminder.Remarks;
            DueDate = reminder.DueDate;
            DueDistance = reminder.DueDistance;
            IsFulfilled = reminder.IsFulfilled;
            IsOverdue = reminder.IsOverdue;
            Update = url.Patch<PatchReminderController>(new {reminder.VehicleId, reminder.ReminderId});
        }

        public string Title { get; set; }
        public string Remarks { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DueDistance { get; set; }
        public bool IsFulfilled { get; set; }
        public bool? IsOverdue { get; set; }
        public object Update { get; set; }
    }
}