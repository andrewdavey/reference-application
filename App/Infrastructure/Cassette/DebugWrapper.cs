using System.Collections.Generic;
using Cassette;

namespace App.Infrastructure.Cassette
{
    class DebugWrapper : StringAssetTransformer
    {
        readonly string modulePath;

        public DebugWrapper(Bundle bundle)
        {
            modulePath = bundle.Path.Substring(2);
        }

        protected override string Transform(string source, IAsset asset)
        {
            var wrapper = new DebugModuleAssetWrapper();
            return wrapper.Wrap(
                source, 
                modulePath, 
                asset.GetImports(),
                asset.GetExportedVariables()
            );
        }
    }
}