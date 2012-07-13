using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cassette;

namespace App.Infrastructure.Cassette
{
    class DebugWrapper : StringAssetTransformer
    {
        readonly IEnumerable<Bundle> bundles;
        readonly string scriptNamespace;

        public DebugWrapper(Bundle bundle, IEnumerable<Bundle> bundles)
        {
            this.bundles = bundles;
            scriptNamespace = bundle.ScriptNamespace();
        }

        string BuildNamespaceInit()
        {
            string[] names = scriptNamespace.Split('.');

            var builder = new StringBuilder();
            builder.AppendFormat("if (typeof {0}==='undefined'){0}={{}};", names[0]);

            var ns = names[0];
            foreach (var name in names.Skip(1))
            {
                ns = ns + "." + name;
                var value = name == names[names.Length - 1] ? "{inits:[]}" : "{}";
                builder.AppendFormat("if (!{0}){0}={1};", ns, value);
            }

            return builder.ToString();
        }

        protected override string Transform(string source, IAsset asset)
        {
            // input:
            //   "var x = ..."
            // output:
            //   if (typeof Pages==='undefined') Pages = {};
            //   if (!Page.Example) Pages.Example = [];
            //   Pages.Example.push(function(bundles,module) {
            //       var import1 = bundles['Other'].import1, import2 = bundles['Other'].import2, import3 = bundles['Other2'].import3;
            //       var x = ...;
            //       module.x = x;
            //   });

            var importsFromOtherBundles = asset.References
                .Where(r => r.Type == AssetReferenceType.DifferentBundle)
                .SelectMany(GetBundleExports)
                .Select(t => t.Item2 + "=bundles['" + t.Item1.ScriptNamespace() + "']." + t.Item2);

            var sameBundleImports = asset.References
                .Where(r => r.Type == AssetReferenceType.SameBundle)
                .SelectMany(GetBundleExports)
                .Select(t => t.Item2 + "=module." + t.Item2);

            var imports = importsFromOtherBundles.Concat(sameBundleImports).ToArray();
            var importVars = imports.Any() ? ("var " + string.Join(",", imports) + ";") : "";

            var exportedVars = asset.GetMetaData<IEnumerable<string>>("ExportedVariables");
            var exports = string.Join(";", exportedVars.Select(v => "module." + v + "=" + v));

            return BuildNamespaceInit() +
                   scriptNamespace + ".inits.push(function(bundles, module){" +
                   importVars +
                   source +
                   "\n" +
                   exports + "});";
        }

        IEnumerable<Tuple<Bundle, string>> GetBundleExports(AssetReference reference)
        {
            var finder = new BundleAssetFinder(reference.ToPath);
            bundles.Accept(finder);

            if (finder.FoundAsset == null)
                throw new Exception("Cannot find bundle containing the asset " + reference.ToPath);

            var exportedVariables = finder.FoundAsset.GetMetaDataOrDefault("ExportedVariables", Enumerable.Empty<string>());
            return exportedVariables.Select(exportedVariable => Tuple.Create(
                finder.FoundBundle,
                exportedVariable
                                                                    ));
        }
    }
}