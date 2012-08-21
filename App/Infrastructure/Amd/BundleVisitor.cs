using System;
using Cassette;

namespace App.Infrastructure.Amd
{
    class BundleVisitor : IBundleVisitor
    {
        public Action<Bundle> VisitBundle = bundle => { };
        public Action<IAsset> VisitAsset = asset => { };

        public void Visit(Bundle bundle)
        {
            VisitBundle(bundle);
        }

        public void Visit(IAsset asset)
        {
            VisitAsset(asset);
        }
    }
}