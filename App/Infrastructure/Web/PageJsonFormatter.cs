using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace App.Infrastructure.Web
{
    public class PageJsonFormatter : JsonMediaTypeFormatter
    {
        public PageJsonFormatter()
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/x-page+json"));
        }
    }

    public class CustomJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public override System.Threading.Tasks.Task WriteToStreamAsync(System.Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            var page = value as Page;
            if (page != null)
            {
                value = page.Data;
            }

            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }
    }
}