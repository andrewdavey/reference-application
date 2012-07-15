using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Cassette.Stylesheets;
using Cassette.Views;

namespace App.Infrastructure.Web
{
    /// <summary>
    /// Page controllers return simple resource objects with just the page data.
    /// This filter wraps these resources in an object that includes metadata about the page,
    /// such as the script module and stylesheet for the page.
    /// </summary>
    public class PageResourceMetadataWrappingFilter : IActionFilter
    {
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var response = await continuation();
            response.Headers.Vary.Add("Content-Type");
            UpdateResponseContent(response, actionContext);
            return response;
        }

        void UpdateResponseContent(HttpResponseMessage response, HttpActionContext actionContext)
        {
            if (!IsPageController(actionContext)) return;

            var objectContent = response.Content as ObjectContent;
            if (objectContent == null) return;

            response.Content = WrapObjectContentWithMetaData(objectContent, actionContext);
        }

        ObjectContent WrapObjectContentWithMetaData(ObjectContent objectContent, HttpActionContext actionContext)
        {
            var bundlePath = ConventionalPageBundlePath(actionContext);
            var value = new
            {
                script = bundlePath,
                stylesheet = Bundles.Url<StylesheetBundle>(bundlePath),
                data = objectContent.Value
            };
            return new ObjectContent(typeof (object), value, objectContent.Formatter);
        }

        string ConventionalPageBundlePath(HttpActionContext actionContext)
        {
            return "Pages/" + actionContext.ControllerContext.ControllerDescriptor.ControllerName;
        }

        bool IsPageController(HttpActionContext actionContext)
        {
            return actionContext
                .ControllerContext
                .ControllerDescriptor
                .ControllerType
                .Namespace
                .Contains(".Pages");
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}