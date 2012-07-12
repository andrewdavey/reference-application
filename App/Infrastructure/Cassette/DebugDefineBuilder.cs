using System.Collections.Generic;
using System.Linq;
using Cassette;
using Newtonsoft.Json;

namespace App.Infrastructure.Cassette
{
    class DebugDefineBuilder : IBundleVisitor
    {
        private readonly IUrlGenerator urlGenerator;
        private readonly List<Shim> shims = new List<Shim>();
        private Shim currentShim;

        public DebugDefineBuilder(IUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public void Visit(Bundle bundle)
        {
            currentShim = new Shim(bundle, urlGenerator);
            shims.Add(currentShim);
        }

        public void Visit(IAsset asset)
        {
            currentShim.Add(asset);
        }

        public override string ToString()
        {
            return string.Join("\n", shims.Select(s=>s.ToString()));
        }

        class Shim
        {
            private readonly Bundle bundle;
            private readonly IUrlGenerator urlGenerator;
            readonly List<IAsset> assets = new List<IAsset>();

            public Shim(Bundle bundle, IUrlGenerator urlGenerator)
            {
                this.bundle = bundle;
                this.urlGenerator = urlGenerator;
            }

            public void Add(IAsset asset)
            {
                assets.Add(asset);
            }

            IEnumerable<string> AssetUrls
            {
                get { return assets.Select(a => urlGenerator.CreateAssetUrl(a)); }
            }

            public override string ToString()
            {
                return "define('" + bundle.ScriptNamespace() + "', " +
                       JsonConvert.SerializeObject(AssetUrls)+ ", " +
                       "function(){return " +bundle.ScriptNamespace() + "})";
            }
        }
    }
}