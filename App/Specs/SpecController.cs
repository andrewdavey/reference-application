using System.Web.Http;
using App.Infrastructure;

namespace App.Specs
{
    public class SpecController : ApiController
    {
        public object GetSpecs()
        {
            return new Page
            {
                Script = "Specs",
                Stylesheet = "Specs",
                Data = new {},
                HtmlFile = "specs.html"
            };
        }
    }
}