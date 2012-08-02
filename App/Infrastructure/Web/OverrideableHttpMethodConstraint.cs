using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace App.Infrastructure.Web
{
    class OverrideableHttpMethodConstraint : HttpMethodConstraint
    {
        public OverrideableHttpMethodConstraint(params HttpMethod[] allowedMethods)
            : base(allowedMethods)
        {   
        }

        protected override bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, System.Collections.Generic.IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
            {
                var method = request.RequestUri.ParseQueryString()["_method"];
                if (method != null)
                {
                    request.Method = new HttpMethod(method);
                }
            }
            return base.Match(request, route, parameterName, values, routeDirection);
        }    
    }
}