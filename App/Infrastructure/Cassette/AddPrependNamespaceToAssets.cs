using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    class AddPrependNamespaceToAssets : AddTransformerToAssets<ScriptBundle>
    {
        protected override IAssetTransformer CreateAssetTransformer(ScriptBundle bundle)
        {
            return new PrependNamespace(bundle);
        }
    }
}