using Cassette;
using Newtonsoft.Json;

namespace App.Infrastructure.Cassette
{
    class ConvertHtmlTemplateToScript : StringAssetTransformer
    {
        protected override string Transform(string source, IAsset asset)
        {
            return "define(function(){" +
                   "return " + JsonConvert.SerializeObject(source) + ";" +
                   "})";
        }
    }
}