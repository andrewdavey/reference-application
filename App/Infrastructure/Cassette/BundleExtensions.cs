using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    static class BundleExtensions
    {
        public static ScriptBundle EmbedHtmlTemplates(this ScriptBundle bundle)
        {
            foreach (var asset in bundle.Assets)
            {
                if (asset.Path.EndsWith(".htm") || asset.Path.EndsWith(".html"))
                {
                    asset.AddReference("~/Infrastructure/Scripts/App/addTemplate.js", 0);
                    asset.AddAssetTransformer(new ConvertHtmlTemplateToScript());
                }
            }
            return bundle;
        }
    }
}