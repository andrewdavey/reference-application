using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Routing;
using HttpMethodConstraint = System.Web.Http.Routing.HttpMethodConstraint;

namespace App.Infrastructure.Web
{
    public static class RoutingExtensions
    {
        public static void Resource(this RouteCollection routes, string name, string urlTemplate)
        {
            var methods = new[] {"Get", "Put", "Post", "Patch", "Delete"};
            foreach (var method in methods)
            {
                routes.MapHttpRoute(
                    method + name,
                    urlTemplate,
                    new { controller = method + name },
                    new { method = new HttpMethodConstraint(new HttpMethod(method.ToUpperInvariant())) }
                );                
            }
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