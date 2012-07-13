using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using App.Infrastructure.Web;
using App.Pages.Vehicles;
using MileageStats.Domain;
using MileageStats.Domain.Handlers;
using App.Infrastructure;

namespace App.Dashboard
{
    public class DashboardController : ApiController
    {
        private readonly GetFleetSummaryStatistics _getFleetSummaryStatistics;

        public DashboardController(GetFleetSummaryStatistics getFleetSummaryStatistics)
        {
            _getFleetSummaryStatistics = getFleetSummaryStatistics;
        }

        public object GetDashboard()
        {
            var statistics = _getFleetSummaryStatistics.Execute(1);
            var resource = new
            {
                statistics,
                vehicles = new { get = Url.Resource<VehiclesController>() }
            };
            
            return resource;
        } 
    }
}