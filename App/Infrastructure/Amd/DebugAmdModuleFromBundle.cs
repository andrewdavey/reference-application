using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;
using Newtonsoft.Json;

namespace App.Infrastructure.Amd
{
    public class DebugAmdModuleFromBundle : AmdModuleFromBundle, IBundleProcessor<ScriptBundle>
    {
        readonly IUrlGenerator urlGenerator;
        readonly IAsset[] originalAssets;

        public DebugAmdModuleFromBundle(ScriptBundle bundle, Func<string, IAmdModule> resolveReferencePathIntoAmdModule, IUrlGenerator urlGenerator)
            : base(bundle, resolveReferencePathIntoAmdModule)
        {
            this.urlGenerator = urlGenerator;
            originalAssets = bundle.Assets.ToArray();
            bundle.Pipeline.Insert(bundle.Pipeline.IndexOf<SortAssetsByDependency>(), this);
        }

        public string DefinitionShim()
        {
            var path = JsonConvert.SerializeObject(Path);

            var assetUrls = new List<string>();
            var visitor = new BundleVisitor
            {
                VisitAsset = asset =>
                {
                    if (!(asset is ShimAsset))
                    {
                        assetUrls.Add(urlGenerator.CreateAssetUrl(asset));
                    }
                }
            };
            Bundle.Accept(visitor);

            var dependenciesAndAssets = JsonConvert.SerializeObject(
                Dependencies.Select(d => d.Path).Concat(assetUrls)
                );

            return string.Format(
                "define({0},{1},function(){{\n" +
                "var exports={{}};\n" +
                "var assets=Array.prototype.slice.call(arguments,{2});\n" +
                "var dependencies=Array.prototype.slice.call(arguments,0,{2});\n" +
                "assets.forEach(function(a) {{ a.apply(exports, dependencies); }});\n" +
                "return exports;\n" +
                "}});",
                path,
                dependenciesAndAssets,
                Dependencies.Length);
        }

        public void Process(ScriptBundle bundle)
        {
            AddDebugAssetWrapperToAssets(bundle);
            AddShimAsset(bundle);
        }

        void AddShimAsset(ScriptBundle bundle)
        {
            bundle.Assets.Add(new ShimAsset(this, urlGenerator));
        }

        void AddDebugAssetWrapperToAssets(ScriptBundle bundle)
        {
            foreach (var asset in bundle.Assets)
            {
                asset.AddAssetTransformer(new DebugAssetWrapper(this));
            }
        }

        public override IEnumerable<KeyValuePair<string, string>> PathMaps(IUrlGenerator urlGenerator)
        {
            var maps = new List<KeyValuePair<string, string>>();

            var visitor = new BundleVisitor
            {
                VisitAsset = asset =>
                {
                    if (asset is ShimAsset)
                    {
                        maps.Add(new KeyValuePair<string, string>(
                            Path,
                            urlGenerator.CreateAssetUrl(asset)
                        ));
                    }
                    else
                    {
                        maps.Add(new KeyValuePair<string, string>(
                            asset.Path.TrimStart('~', '/'),
                            urlGenerator.CreateAssetUrl(asset)
                        ));
                    }
                }
            };
            Bundle.Accept(visitor);

            return maps;
        }
    }
}