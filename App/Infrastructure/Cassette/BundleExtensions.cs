using System;
using System.Linq;
using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    static class BundleExtensions
    {
        public static ScriptBundle AmdModule(this ScriptBundle bundle)
        {
            var sortIndex = bundle.Pipeline.IndexOf<SortAssetsByDependency>();

            bundle.Pipeline.Insert<RecordExportedVariables>(sortIndex + 1);
            bundle.Pipeline.Insert<AddDebugWrapperToAssets>(sortIndex + 2);

            return bundle;
        }

        public static ScriptBundle AmdModulePerAsset(this ScriptBundle bundle)
        {
            bundle.SetMetaData("AmdModulePerAsset", true);
            //bundle.Pipeline.Insert<AddTransformerToAssets<RewriteDefineCalls, ScriptBundle>>(0);
            return bundle;
        }

        public static ScriptBundle AmdAlias(this ScriptBundle bundle, string assetFilename, string alias)
        {
            var assetToAlias = bundle.Assets.First(asset => asset.Path.EndsWith(assetFilename, StringComparison.OrdinalIgnoreCase));
            assetToAlias.SetMetaData("AmdAlias", alias);
            return bundle;
        }

        public static ScriptBundle EmbedHtmlTemplates(this ScriptBundle bundle)
        {
            bundle.Pipeline.Insert<ConvertAllHtmlTemplatesToScript>(0);
            return bundle;
        }

        public static string ScriptNamespace(this Bundle bundle)
        {
            return bundle
                .Path
                .Substring(2)
                .Replace('/', '.');
        }
    }
}