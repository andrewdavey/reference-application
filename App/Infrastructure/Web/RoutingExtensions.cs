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

        public static void PutResource<T>(this RouteCollection routes, string urlTemplate)
        {
            var name = ControllerName<T>();
            routes.MapHttpRoute(
                name,
                urlTemplate,
                new { controller = name },
                new { get = new System.Web.Http.Routing.HttpMethodConstraint(new HttpMethod("PUT")) }
            );
        }

        public static string Resource<T>(this UrlHelper url, object routeValues = null)
        {
            var name = ControllerName<T>();
            return url.Route(name, routeValues);
        }

        public static object Get<T>(this UrlHelper url, object routeValues = null)
        {
            return HttpLink<T>(url, "get", routeValues);
        }

        public static object Put<T>(this UrlHelper url, object routeValues = null)
        {
            return HttpLink<T>(url, "put", routeValues);
        }

        public static object Patch<T>(this UrlHelper url, object routeValues = null)
        {
            return HttpLink<T>(url, "patch", routeValues);
        }

        public static object Post<T>(this UrlHelper url, object routeValues = null)
        {
            return HttpLink<T>(url, "post", routeValues);
        }

        public static object Delete<T>(this UrlHelper url, object routeValues = null)
        {
            return HttpLink<T>(url, "delete", routeValues);
        }

        static object HttpLink<T>(UrlHelper url, string httpMethod, object routeValues = null)
        {
            return new
            {
                method = httpMethod,
                url = url.Resource<T>(routeValues)
            };
        }

        static string ControllerName<T>()
        {
            var name = typeof (T).Name;
            name = name.Substring(0, name.Length - "Controller".Length);
            return name;
        }
    }
}