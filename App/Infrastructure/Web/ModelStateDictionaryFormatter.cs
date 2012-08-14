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
            // Convert the model state dictionary into a simpler format that will serialize nicely into JSON.
            // Output is something like:
            //     { propertyA: [ "First error", "Second Error" ], propertyB: [ "Another error" ] }
            
            // Also, property names are converted to camelCase to make mapping back to client-side properties easier.

            var modelState = (ModelStateDictionary)value;
            var objectToSend = modelState
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(
                    x => CamelCase(x.Key),
                    x => x.Value.Errors.Select(e => e.ErrorMessage)
                );
            return base.WriteToStreamAsync(type, objectToSend, stream, contentHeaders, transportContext);
        }

        static string CamelCase(string input)
        {
            return input.Length > 0 
                ? (input.Substring(0,1).ToLowerInvariant() + input.Substring(1)) 
                : input;
        }
    }
}