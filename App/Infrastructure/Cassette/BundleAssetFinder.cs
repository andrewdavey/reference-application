using System;
using Cassette;

namespace App.Infrastructure.Cassette
{
    class BundleAssetFinder : IBundleVisitor
    {
        private readonly string assetPathToFind;
        private Bundle currentBundle;

        public BundleAssetFinder(string assetPathToFind)
        {
            this.assetPathToFind = assetPathToFind;
        }

        public void Visit(Bundle bundle)
        {
            currentBundle = bundle;
        }

        public void Visit(IAsset asset)
        {
            if (asset.Path.Equals(assetPathToFind, StringComparison.OrdinalIgnoreCase))
            {
                FoundBundle = currentBundle;
                FoundAsset = asset;
            }
        }

        public IAsset FoundAsset { get; private set; }
        public Bundle FoundBundle { get; private set; }
    }
}