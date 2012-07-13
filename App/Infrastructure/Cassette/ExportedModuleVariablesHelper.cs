using System.Collections.Generic;
using System.Linq;
using Cassette;

namespace App.Infrastructure.Cassette
{
    public static class ExportedModuleVariablesHelper
    {
        const string MetaDataKey = "ExportedVariables";

        public static void RecordExportedVariables(this IAsset asset, string source)
        {
            var variables = GlobalJavaScriptVariableParser.GetVariables(source);
            asset.SetMetaData(MetaDataKey, variables);
        }

        public static IEnumerable<string> GetExportedVariables(this IAsset asset)
        {
            return asset.GetMetaDataOrDefault(MetaDataKey, Enumerable.Empty<string>());
        }
    }
}