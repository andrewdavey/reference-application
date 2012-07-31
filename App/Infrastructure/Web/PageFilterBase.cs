using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace App.Infrastructure.Web
{
    public abstract class PageFilterBase : IActionFilter
    {
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var response = await continuation();

            var content = response.Content as ObjectContent;
            if (content != null)
            {
                var page = content.Value as Page;
                if (page != null)
                {
                    Execute(page, actionContext, response);
                }
            }

            return response;
        }

        protected abstract void Execute(Page page, HttpActionContext actionContext, HttpResponseMessage response);

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}