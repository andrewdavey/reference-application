using System.Web.Http;
using App.Infrastructure.Web;
using App.Modules.Vehicles;
using MileageStats.Domain.Handlers;

namespace App.Pages.Dashboard
{
    public class DashboardController : ApiController
    {
        readonly GetFleetSummaryStatistics getFleetSummaryStatistics;

        public DashboardController(GetFleetSummaryStatistics getFleetSummaryStatistics)
        {
            this.getFleetSummaryStatistics = getFleetSummaryStatistics;
        }

        public object GetDashboard()
        {
            var statistics = getFleetSummaryStatistics.Execute(1);
            var resource = new
            {
                statistics,
                vehicles = new { get = Url.Resource<VehiclesController>() }
            };
            
            return resource;
        } 
    }
}