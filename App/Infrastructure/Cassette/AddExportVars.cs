using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    class AddExportVars : AddTransformerToAssets<ScriptBundle>
    {
        protected override IAssetTransformer CreateAssetTransformer(ScriptBundle bundle)
        {
            return new ExportVars(bundle);
        }
    }
}