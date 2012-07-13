using System.Collections.Generic;
using Cassette;
using Cassette.BundleProcessing;

namespace App.Infrastructure.Cassette
{
    class AddDebugWrapperToAssets : AddTransformerToAssets<Bundle>
    {
        readonly IEnumerable<Bundle> bundles;

        public AddDebugWrapperToAssets(BundleCollection bundles)
        {
            this.bundles = bundles;
        }

        protected override IAssetTransformer CreateAssetTransformer(Bundle bundle)
        {
            return new DebugWrapper(bundle, bundles);
        }
    }
}