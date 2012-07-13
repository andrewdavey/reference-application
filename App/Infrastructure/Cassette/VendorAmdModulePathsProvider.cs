using System.Collections.Generic;
using System.Linq;
using Cassette;

namespace App.Infrastructure.Cassette
{
    public class VendorAmdModulePathsProvider : IStartUpTask
    {
        readonly BundleCollection bundles;
        readonly IUrlGenerator urlGenerator;

        public static string Paths { get; private set; }

        public VendorAmdModulePathsProvider(BundleCollection bundles, IUrlGenerator urlGenerator)
        {
            this.bundles = bundles;
            this.urlGenerator = urlGenerator;
        }

        public void Start()
        {
            bundles.Changed += BundlesOnChanged;
        }

        private void BundlesOnChanged(object sender, BundleCollectionChangedEventArgs bundleCollectionChangedEventArgs)
        {
            var vendorBundles = bundleCollectionChangedEventArgs
                .Bundles
                .Where(b => MetaData.GetMetaDataOrDefault(b, "AmdModulePerAsset", false));

            var collector = new CollectAssets();
            vendorBundles.Accept(collector);

            var paths =
                collector.Assets.Select(
                    asset =>
                    new
                    {
                        path = AssetPath(asset),
                        url = urlGenerator.CreateAssetUrl(asset)
                    })
                    .Select(x => string.Format("'{0}':'{1}'", x.path, x.url));

            Paths = string.Join(",\n", paths);
        }

        string AssetPath(IAsset asset)
        {
            if (asset.Path.EndsWith("jquery.js")) return "jquery";
            return asset.Path.Substring(2).TrimEnd('.','j','s');
        }

        class CollectAssets : IBundleVisitor
        {
            public CollectAssets()
            {
                Assets = new List<IAsset>();
            }

            public List<IAsset> Assets { get; private set; }

            public void Visit(Bundle bundle)
            {
            }

            public void Visit(IAsset asset)
            {
                Assets.Add(asset);
            }
        }
    }
}