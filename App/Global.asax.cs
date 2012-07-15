using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Routing;
using App.Infrastructure.Unity;
using App.Infrastructure.Web;
using Cassette.Stylesheets;
using Cassette.Views;
using Microsoft.Practices.Unity;
using MileageStats.Data.InMemory;
using Unity.WebApi;

namespace App
{
    public class MvcApplication : HttpApplication
    {
        static IUnityContainer _container;

        protected void Application_Start()
        {
            InitializeDependencyInjectionContainer();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            _container.Resolve<PopulateSampleData>().Seed(null);

            GlobalConfiguration.Configuration.Formatters.Add(new HtmlFormatter());
            GlobalConfiguration.Configuration.Filters.Add(new PageFilter());
        }

        public override void Init()
        {
            EndRequest += EndRequestHandler;

            base.Init();
        }

        void EndRequestHandler(object sender, EventArgs e)
        {
            // This is a workaround since subscribing to HttpContext.Current.ApplicationInstance.EndRequest 
            // from HttpContext.Current.ApplicationInstance.BeginRequest does not work. 
            IEnumerable<UnityHttpContextPerRequestLifetimeManager> perRequestManagers =
                _container.Registrations
                    .Select(r => r.LifetimeManager)
                    .OfType<UnityHttpContextPerRequestLifetimeManager>()
                    .ToArray();

            foreach (var manager in perRequestManagers)
            {
                manager.Dispose();
            }
        }

        void InitializeDependencyInjectionContainer()
        {
            _container = new UnityContainerFactory().CreateConfiguredContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(_container);
        }
    }

    public class PageFilter : IActionFilter
    {
        public bool AllowMultiple { get; private set; }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var response = await continuation();
            response.Headers.Vary.Add("Content-Type");

            var objectContent = response.Content as ObjectContent;
            if (objectContent != null && actionContext.ControllerContext.ControllerDescriptor.ControllerType.Namespace.Contains(".Pages"))
            {
                var bundle = "Pages/" + actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                var value = new
                {
                    script = bundle,
                    stylesheet = Bundles.Url<StylesheetBundle>(bundle),
                    data = objectContent.Value
                };
                var pageObject = new ObjectContent(typeof (object), value, objectContent.Formatter);
                response.Content = pageObject;
            }

            return response;
        }
    }
}