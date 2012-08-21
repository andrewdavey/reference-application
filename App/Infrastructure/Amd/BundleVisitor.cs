using System;
using Cassette;

namespace App.Infrastructure.Amd
{
    class BundleVisitor : IBundleVisitor
    {
        readonly Action<IAsset> visitAsset;

        public BundleVisitor(Action<IAsset> visitAsset)
        {
            if (visitAsset == null) throw new ArgumentNullException("visitAsset");

            this.visitAsset = visitAsset;
        }

        public void Visit(Bundle bundle)
        {
        }

        public void Visit(IAsset asset)
        {
            visitAsset(asset);
        }
    }
}