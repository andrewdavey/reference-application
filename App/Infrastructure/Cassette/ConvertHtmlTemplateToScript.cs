using Cassette;
using Cassette.Scripts;
using Newtonsoft.Json;

namespace App.Infrastructure.Cassette
{
    class ConvertHtmlTemplateToScript : StringAssetTransformer
    {
        private readonly ScriptBundle bundle;

        public ConvertHtmlTemplateToScript(ScriptBundle bundle)
        {
            this.bundle = bundle;
        }

        protected override string Transform(string source, IAsset asset)
        {
            var templates = bundle.ScriptNamespace() + ".templates";
            return templates + "['" + asset.Path.Substring(2) + "']=" + JsonConvert.SerializeObject(source);
        }
    }
}