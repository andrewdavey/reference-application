using System.Collections.Generic;
using Cassette;
using Cassette.BundleProcessing;

namespace App.Infrastructure.Cassette
{
    class AddDebugWrapperToAssets : IBundleProcessor<Bundle>
    {
        public void Process(Bundle bundle)
        {
            var index = 0;
            foreach (var asset in bundle.Assets)
            {
                asset.AddAssetTransformer(new DebugWrapper(bundle, index++));
            }
        }
    }
}