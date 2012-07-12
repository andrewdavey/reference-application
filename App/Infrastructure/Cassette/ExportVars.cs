using System.Collections.Generic;
using System.Linq;
using Cassette;

namespace App.Infrastructure.Cassette
{
    class ExportVars : StringAssetTransformer
    {
        private readonly string scriptNamespace;

        public ExportVars(Bundle bundle)
        {
            scriptNamespace = bundle.ScriptNamespace();
        }

        protected override string Transform(string source, IAsset asset)
        {
            var variables = asset.GetMetaData<IEnumerable<string>>("ExportedVariables");
            var exports = variables.Select(v => string.Format("{0}.{1}={1};", scriptNamespace, v));
            return source + "\n" + string.Join("\n", exports);
        }
    }
}