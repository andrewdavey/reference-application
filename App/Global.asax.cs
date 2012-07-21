using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using App.Infrastructure.Unity;
using Microsoft.Practices.Unity;
using MileageStats.Data.InMemory;

namespace App
{
    public class MvcApplication : HttpApplication
    {
        static IUnityContainer _container;

        protected void Application_Start()
        {
            _container = CreateContainer();

            PopulateSampleData();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Initialize(_container);
        }

        IUnityContainer CreateContainer()
        {
            return new UnityContainerFactory().CreateConfiguredContainer();
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

        void PopulateSampleData()
        {
            _container.Resolve<PopulateSampleData>().Seed(null);
        }
    }
}