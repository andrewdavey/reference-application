using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Profile;
using App.Vehicles;
using MileageStats.Domain.Handlers;

namespace App.Dashboard
{
    public class DashboardController : ApiController
    {
        readonly GetFleetSummaryStatistics getFleetSummaryStatistics;
        readonly GetImminentRemindersForUser getImminentReminders;

        public DashboardController(GetFleetSummaryStatistics getFleetSummaryStatistics, GetImminentRemindersForUser getImminentReminders)
        {
            this.getFleetSummaryStatistics = getFleetSummaryStatistics;
            this.getImminentReminders = getImminentReminders;
        }

        public object GetDashboard()
        {
            var statistics = getFleetSummaryStatistics.Execute(1);
            var reminders = GetReminders();

            var resource = new Page
            {
                Script = "Dashboard",
                Stylesheet = "Dashboard",
                Data = new
                {
                    statistics,
                    reminders,
                    vehicles = Url.Get<VehiclesController>(),
                    addVehicle = Url.Get<GetNewVehicleController>(),
                    profile = Url.Get<ProfileController>()
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
                    href = "#",
                    title = r.Reminder.Title,
                    due = r.Reminder.DueOnFormatted,
                    vehicleMake = r.VehicleMakeName,
                    vehicleModel = r.VehicleModelName,
                    isOverdue = r.IsOverdue
                });
        }
    }
}