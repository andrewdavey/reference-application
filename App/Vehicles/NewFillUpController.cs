using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;

namespace App.Vehicles
{
    public class NewFillUpController : ApiController
    {
        public object GetPage()
        {
            return new Page
            {
                Title = "Add fill up",
                Script = "Vehicles/NewFillUpPage",
                Data = new
                {
                    save = Url.Post<FillUpsController>()
                }
            };
        }
    }
}