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
                HtmlFile = "specs.html"
            };
        }
    }
}