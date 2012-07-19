using Cassette;
using Newtonsoft.Json;

namespace App.Infrastructure.Cassette
{
    class ConvertHtmlTemplateToScript : StringAssetTransformer
    {
        protected override string Transform(string source, IAsset asset)
        {
            return "deps['Infrastructure/Scripts/App'].addTemplate(" +
                   JsonConvert.SerializeObject(asset.Path.Substring(2)) + "," +
                   JsonConvert.SerializeObject(source) + 
                   ")";
        }
    }
}