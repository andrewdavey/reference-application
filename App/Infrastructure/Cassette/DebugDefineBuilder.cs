using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;
using Newtonsoft.Json;

namespace App.Infrastructure.Cassette
{
    class DebugDefineBuilder : IBundleVisitor
    {
        readonly IUrlGenerator urlGenerator;
        readonly Func<string, Bundle> bundleFromPath;
        readonly List<Shim> shims = new List<Shim>();
        Shim currentShim;

        public DebugDefineBuilder(IUrlGenerator urlGenerator, Func<string, Bundle> bundleFromPath)
        {
            this.urlGenerator = urlGenerator;
            this.bundleFromPath = bundleFromPath;
        }

        public void Visit(Bundle bundle)
        {
            currentShim = new Shim(bundle, urlGenerator, bundleFromPath);
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
            private readonly Func<string, Bundle> bundleFromPath;
            readonly List<IAsset> assets = new List<IAsset>();

            public Shim(Bundle bundle, IUrlGenerator urlGenerator, Func<string, Bundle> bundleFromPath)
            {
                this.bundle = bundle;
                this.urlGenerator = urlGenerator;
                this.bundleFromPath = bundleFromPath;
            }

            public void Add(IAsset asset)
            {
                assets.Add(asset);
            }

            IEnumerable<string> AssetUrls
            {
                get { return assets.Select(a => urlGenerator.CreateAssetUrl(a)); }
            }

            IEnumerable<string> BundleReferences
            {
                get
                {
                    return from asset in assets
                           from reference in asset.References
                           where reference.Type == AssetReferenceType.DifferentBundle
                           select bundleFromPath(reference.ToPath).ScriptNamespace();
                }
            }

            IEnumerable<string> AllReferences
            {
                get { return BundleReferences.Concat(AssetUrls); }
            }

            string Parameters
            {
                get { return string.Join(",", BundleReferences.Select(r => r.Replace('.', '_'))); }
            }

            public override string ToString()
            {
                return "define('" + bundle.ScriptNamespace() + "', " +
                       JsonConvert.SerializeObject(AllReferences) + ", " +
                       "function(" + Parameters + "){" +
                       "var module = {};" +
                       "var bundles = {" +
                       string.Join(",", BundleReferences.Select((r, i) => "'" + r + "':arguments[" + i + "]")) +
                       "};" +
                       bundle.ScriptNamespace() + ".inits.forEach(function(asset) { asset.call(null,bundles,module); });" +
                       "return module; })";
            }
        }
    }
}