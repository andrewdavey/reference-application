using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace App.Infrastructure.Web
{
    public class ModelStateDictionaryFormatter : JsonMediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(ModelStateDictionary);
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, string mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/x-validation-errors+json");
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext transportContext)
        {
            var modelState = (ModelStateDictionary)value;
            var objectToSend = modelState
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(
                    x => x.Key.Length > 0 ? (x.Key.Substring(0,1).ToLowerInvariant() + x.Key.Substring(1)) : x.Key,
                    x => x.Value.Errors.Select(e => e.ErrorMessage)
                );
            return base.WriteToStreamAsync(type, objectToSend, stream, contentHeaders, transportContext);
        }
    }
}