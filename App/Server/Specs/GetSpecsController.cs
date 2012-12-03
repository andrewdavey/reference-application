using System.Web.Http;
using App.Infrastructure;

namespace App.Server.Specs
{
    public class GetSpecsController : ApiController
    {
        public object GetSpecs()
        {
            return new Page("Specs")
            {
                InitializationModule = "Client/Specs/init",
                Stylesheets = new[] { "/Client/Specs/jasmine/jasmine.css" }
            };
        }
    }
}