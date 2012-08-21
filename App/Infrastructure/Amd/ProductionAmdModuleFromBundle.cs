using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;
using Newtonsoft.Json;

namespace App.Infrastructure.Amd
{
    public class ProductionAmdModuleFromBundle : AmdModuleFromBundle, IBundleProcessor<ScriptBundle>
    {
        readonly IUrlGenerator urlGenerator;

        public ProductionAmdModuleFromBundle(ScriptBundle bundle, Func<string, IAmdModule> resolveReferencePathIntoAmdModule, IUrlGenerator urlGenerator)
            : base(bundle, resolveReferencePathIntoAmdModule)
        {
            this.urlGenerator = urlGenerator;
            WillWrapConcatenatedAssetsInDefineCall(bundle);
        }

        void WillWrapConcatenatedAssetsInDefineCall(ScriptBundle bundle)
        {
            var afterConcatenateAssets = 1 + bundle.Pipeline.IndexOf<ConcatenateAssets>();
            bundle.Pipeline.Insert(afterConcatenateAssets, this);
        }

        public void Process(ScriptBundle bundle)
        {
            bundle.Assets[0].AddAssetTransformer(new Wrap(this));
        }

        public string WrapScriptInDefineCall(string source)
        {
            var path = JsonConvert.SerializeObject(Path);
            var dependencies = JsonConvert.SerializeObject(Dependencies.Select(d => d.Path));
            var parameters = string.Join(",", Dependencies.Select(d => d.Export.Identifier));
            var aliases = string.Join("", from d in Dependencies
                                          from a in d.Export.Aliases
                                          select string.Format("var {0}={1}.{0};", a, d.Export.Identifier));
            
            var exports = "{" + string.Join(",", Export.Aliases.Select(a => a + ":" + a)) + "}";

            var start = string.Format("define({0},{1},function({2}){{{3}", path, dependencies, parameters, aliases);
            var end = string.Format("\nreturn {0};\n}});", exports);
            return start + source + end;
        }

        class Wrap : StringAssetTransformer
        {
            readonly ProductionAmdModuleFromBundle module;

            public Wrap(ProductionAmdModuleFromBundle module)
            {
                this.module = module;
            }

            protected override string Transform(string source, IAsset asset)
            {
                return module.WrapScriptInDefineCall(source);
            }
        }

        public override IEnumerable<KeyValuePair<string, string>> PathMaps()
        {
            yield return new KeyValuePair<string, string>(Path, urlGenerator.CreateBundleUrl(Bundle));
        }
    }
}