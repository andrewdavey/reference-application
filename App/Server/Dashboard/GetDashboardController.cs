using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Server.Profile;
using App.Server.Vehicle;
using MileageStats.Domain.Handlers;

namespace App.Server.Dashboard
{
    public class GetDashboardController : ApiController
    {
        readonly GetFleetSummaryStatistics getFleetSummaryStatistics;
        readonly GetImminentRemindersForUser getImminentReminders;

        public GetDashboardController(GetFleetSummaryStatistics getFleetSummaryStatistics, GetImminentRemindersForUser getImminentReminders)
        {
            this.getFleetSummaryStatistics = getFleetSummaryStatistics;
            this.getImminentReminders = getImminentReminders;
        }

        public object GetDashboard()
        {
            var statistics = getFleetSummaryStatistics.Execute(1);
            var reminders = GetReminders();

            var resource = new Page("Dashboard")
            {
                Title = "MileageStats Sample Application",
                Data = new
                {
                    statistics,
                    reminders,
                    vehicles = Url.Get<GetVehiclesController>(),
                    addVehicle = Url.Get<GetNewVehicleController>(),
                    profile = Url.Get<GetProfileController>()
                }
            };
            
            return resource;
        }

        IEnumerable<object> GetReminders()
        {
            return getImminentReminders
                .Execute(1, DateTime.UtcNow)
                .Select(r => new
                {
                    href = "#", // TODO: Make this a real URL
                    title = r.Reminder.Title,
                    due = r.Reminder.DueOnFormatted,
                    vehicleMake = r.VehicleMakeName,
                    vehicleModel = r.VehicleModelName,
                    isOverdue = r.IsOverdue
                });
        }
    }
}