using Cassette.BundleProcessing;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    class ConvertAllHtmlTemplatesToScript : IBundleProcessor<ScriptBundle>
    {
        public void Process(ScriptBundle bundle)
        {
            foreach (var asset in bundle.Assets)
            {
                if (asset.Path.EndsWith(".htm") || asset.Path.EndsWith(".html"))
                {
                    asset.AddAssetTransformer(new ConvertHtmlTemplateToScript());
                }
            }
        }
    }
}