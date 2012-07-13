using System.Collections.Generic;
using Cassette;
using Cassette.BundleProcessing;

namespace App.Infrastructure.Cassette
{
    class AddDebugWrapperToAssets : AddTransformerToAssets<Bundle>
    {
        protected override IAssetTransformer CreateAssetTransformer(Bundle bundle)
        {
            return new DebugWrapper(bundle);
        }
    }
}