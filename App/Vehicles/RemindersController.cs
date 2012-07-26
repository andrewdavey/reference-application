using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using App.Vehicles.VehicleMasterPage;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class RemindersController : ApiController
    {
        readonly GetAllRemindersForVehicle getAllRemindersForVehicle;

        public RemindersController(GetAllRemindersForVehicle getAllRemindersForVehicle)
        {
            this.getAllRemindersForVehicle = getAllRemindersForVehicle;
        }

        public object GetReminders(int id)
        {
            var reminders = getAllRemindersForVehicle.Execute(id);
            
            return new Page
            {
                Master = Url.Resource<VehicleMasterPageController>(),
                Script = "Vehicles/Reminders",
                Data = new
                {
                    reminders
                }
            };
        }
    }
}
