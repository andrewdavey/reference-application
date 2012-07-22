using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace App.Infrastructure.Web
{
    public class PageLanguageFilter : IActionFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var response = await continuation();

            var content = response.Content as ObjectContent;
            if (content != null)
            {
                var page = content.Value as Page;
                if (page != null)
                {
                    var language = actionContext.Request.Headers.AcceptLanguage.Select(l => l.Value).FirstOrDefault();
                    page.Language = language ?? "en-US";
                }
            }

            return response;
        }
    }
}