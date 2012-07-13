using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using App.Infrastructure.Cassette;

namespace App.Infrastructure.Web
{
    public class HtmlFormatter : MediaTypeFormatter
    {
        public HtmlFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xhtml+xml"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override async System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, System.Net.TransportContext transportContext)
        {
            contentHeaders.ContentType = new MediaTypeHeaderValue("text/html");

            var filename = Path.Combine(HttpRuntime.AppDomainAppPath, "app.html");
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    var html = await reader.ReadToEndAsync();
                    html = html.Replace("$paths$", VendorAmdModulePathsProvider.Paths);
                    html = html.Replace("$shims$", DebugModuleDefinitionsProvider.ModuleDefinitions);

                    var bytes = Encoding.UTF8.GetBytes(html);
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
        }
    }
}