using System.Web.Http;
using App.Infrastructure;

namespace App.Specs
{
    public class GetSpecsController : ApiController
    {
        public object GetSpecs()
        {
            return new Page("Specs")
            {
                Data = new {},
                HtmlFile = "specs.html"
            };
        }
    }
}