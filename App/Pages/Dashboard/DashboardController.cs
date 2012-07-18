using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using App.Modules.Vehicles;
using App.Pages.GetNewVehicle;
using MileageStats.Domain.Handlers;
using MileageStats.Domain.Models;

namespace App.Pages.Dashboard
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

            var resource = new
            {
                statistics,
                reminders,
                vehicles = new { get = Url.Resource<VehiclesController>() },
                addVehicle = Url.Resource<GetNewVehicleController>()
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