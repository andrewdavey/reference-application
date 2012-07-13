using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;

namespace App.Infrastructure.Cassette
{
    class DebugModuleCollectionBuilder : IBundleVisitor
    {
        readonly IUrlGenerator urlGenerator;
        readonly List<DebugModuleBuilder> builders = new List<DebugModuleBuilder>();
        DebugModuleBuilder currentBuilder;

        public DebugModuleCollectionBuilder(IUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public void Visit(Bundle bundle)
        {
            currentBuilder = new DebugModuleBuilder(bundle.Path.Substring(2));

            builders.Add(currentBuilder);
        }

        public void Visit(IAsset asset)
        {
            currentBuilder.AddAssetUrl(urlGenerator.CreateAssetUrl(asset));
            currentBuilder.AddDependencies(GetDependencies(asset));
        }

        IEnumerable<string> GetDependencies(IAsset asset)
        {
            var imports = asset.GetImports();
            return Import.GetAllModulePaths(imports);
        }

        public string Build()
        {
            return "var debugModules = {};" + string.Join("\r\n", builders.Select(b => b.Build()));
        }
    }
}