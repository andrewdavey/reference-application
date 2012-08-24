using System.Web.Http;
using App.Infrastructure;

namespace App.Server
{
    public class GetAppFrameController : ApiController
    {
        public object GetAppFrame()
        {
            return new Page("AppFrame")
            {
                Data = new
                {
                    links = new[]
                    {
                        new {text = "Dashboard", url = "/"},
                        new {text = "Log Out", url = "#"}
                    }
                }
            };
        }
    }
}