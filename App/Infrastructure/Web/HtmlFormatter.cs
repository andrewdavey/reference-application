using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using App.Infrastructure.Amd;
using App.Infrastructure.Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;
using Cassette.Views;
using Newtonsoft.Json;

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

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("text/html");
        }

        public async override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var page = value as Page;
            if (page != null)
            {
                await WritePageToStreamAsync(writeStream, page);
            }
            else
            {
                await WriteIFrameDataToStreamAsync(writeStream, value);
            }
        }

        async Task WritePageToStreamAsync(Stream writeStream, Page page)
        {
            var filename = Path.Combine(HttpRuntime.AppDomainAppPath, page.HtmlFile ?? "app.html");
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(file))
            {
                Bundles.Reference<StylesheetBundle>("Client/Vendor");
                Bundles.Reference<ScriptBundle>("Client/Vendor");

                var html = await reader.ReadToEndAsync();

                html = html.Replace("$lang$", page.Language);
                html = html.Replace("$styles$", Bundles.RenderStylesheets().ToHtmlString());
                html = html.Replace("$scripts$", Bundles.RenderScripts().ToHtmlString());
                html = html.Replace("$styleMap$", StylesheetPathProvider.PathMapJson);
                html = html.Replace("$requirejson$", JsonConvert.SerializeObject(AmdModuleCollection.Instance.Require));

                var bytes = Encoding.UTF8.GetBytes(html);
                await writeStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        async Task WriteIFrameDataToStreamAsync(Stream writeStream, object value)
        {
            value = value ?? new object();
            var writer = new StreamWriter(writeStream);
            await writer.WriteAsync(
                "<!DOCTYPE html>\n<html><body>" + 
                JsonConvert.SerializeObject(value) + 
                "</body></html>"
            );
            await writer.FlushAsync();
        }
    }
}