using System.Collections.Generic;
using Cassette;

namespace App.Infrastructure.Cassette
{
    class DebugWrapper : StringAssetTransformer
    {
        readonly int assetIndex;
        readonly string modulePath;

        public DebugWrapper(Bundle bundle, int assetIndex)
        {
            this.assetIndex = assetIndex;
            modulePath = bundle.Path.TrimStart('~', '/');
        }

        protected override string Transform(string source, IAsset asset)
        {
            var wrapper = new DebugModuleAssetWrapper();
            return wrapper.Wrap(
                source, 
                modulePath, 
                asset.GetImports(),
                asset.GetExportedVariables(),
                assetIndex
            );
        }
    }
}