using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace App.Infrastructure.Web
{
    public static class RoutingExtensions
    {
        public static void MapResource<T>(this RouteCollection routes, string urlTemplate)
        {
            var name = ControllerName<T>();
            routes.MapHttpRoute(name, urlTemplate, new {controller = name});
        }

        public static void GetResource<T>(this RouteCollection routes, string urlTemplate)
        {
            var name = ControllerName<T>();
            routes.MapHttpRoute(
                name,
                urlTemplate,
                new { controller = name },
                new{get = new System.Web.Http.Routing.HttpMethodConstraint(new HttpMethod("GET"))}
            );
        }

        public static void PostResource<T>(this RouteCollection routes, string urlTemplate)
        {
            var name = ControllerName<T>();
            routes.MapHttpRoute(
                name,
                urlTemplate,
                new { controller = name },
                new { get = new System.Web.Http.Routing.HttpMethodConstraint(new HttpMethod("POST")) }
            );
        }

        public static string Resource<T>(this UrlHelper url, object routeValues = null)
        {
            var name = ControllerName<T>();
            return url.Link(name, routeValues);
        }

        private static string ControllerName<T>()
        {
            var name = typeof (T).Name;
            name = name.Substring(0, name.Length - "Controller".Length);
            return name;
        }
    }
}