using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace App.Infrastructure.Web
{
    public class PageLanguageFilter : PageFilterBase
    {
        const string DefaultLanguage = "en-US";

        protected override void Execute(Page page, HttpActionContext actionContext, HttpResponseMessage response)
        {
            var language = actionContext.Request.Headers.AcceptLanguage.Select(l => l.Value).FirstOrDefault();
            page.Language = language ?? DefaultLanguage;
        }
    }
}