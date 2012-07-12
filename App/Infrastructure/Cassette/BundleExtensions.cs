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

            bundle.Pipeline.Insert<RecordGlobalVariables>(sortIndex);
            bundle.Pipeline.Insert<AddPrependNamespaceToAssets>(sortIndex + 1);
            bundle.Pipeline.Insert<AddExportVars>(sortIndex + 2);
            bundle.Pipeline.Insert<AddIifeWrappers>(sortIndex + 3);

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