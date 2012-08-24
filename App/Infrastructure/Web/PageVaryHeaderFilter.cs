using System.Net.Http;
using System.Web.Http.Controllers;

namespace App.Infrastructure.Web
{
    public class PageVaryHeaderFilter : PageFilterBase
    {
        protected override void Execute(Page page, HttpActionContext actionContext, HttpResponseMessage response)
        {
            VaryByContentType(response);
        }

        void VaryByContentType(HttpResponseMessage response)
        {
            // Vary by Accept so that browsers cache HTML and JSON responses separately.
            response.Headers.Vary.Add("Accept");
        }
    }
}