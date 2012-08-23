using System.Web.Http;
using App.Infrastructure.Web;
using Microsoft.Practices.Unity;
using Unity.WebApi;

namespace App
{
    public static class WebApiConfig
    {
        public static void Initialize(IUnityContainer container)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            GlobalConfiguration.Configuration.Formatters.Add(new HtmlFormatter());
            GlobalConfiguration.Configuration.Formatters.Insert(0, new ModelStateDictionaryFormatter());
            GlobalConfiguration.Configuration.Filters.Add(new PageLanguageFilter());
            GlobalConfiguration.Configuration.Filters.Add(new PageVaryHeaderFilter());
            GlobalConfiguration.Configuration.Filters.Add(new PageConventionsFilter());
        }
    }
}