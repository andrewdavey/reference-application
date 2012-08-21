using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cassette;
using Cassette.Scripts;

namespace App.Infrastructure.Amd
{
    public class AmdModuleFromBundle : IAmdModule
    {
        readonly ScriptBundle bundle;
        readonly Func<string, IAmdModule> resolveReferencePathIntoAmdModule;
        readonly Lazy<IAmdModule[]> dependencies;
        readonly Dictionary<IAsset, IEnumerable<string>> exportsByAsset = new Dictionary<IAsset, IEnumerable<string>>();
        readonly List<string> allExports = new List<string>(); 
        readonly Dictionary<IAsset, string> originalSources = new Dictionary<IAsset, string>();

        public AmdModuleFromBundle(ScriptBundle bundle, Func<string, IAmdModule> resolveReferencePathIntoAmdModule)
        {
            this.bundle = bundle;
            this.resolveReferencePathIntoAmdModule = resolveReferencePathIntoAmdModule;
            Path = bundle.Path.TrimStart('~', '/');
            // Dependencies must be lazy because resolveReferencePathIntoAmdModule 
            // may need to return a module that hasn't yet been created.
            // Lazy means we can avoid parsing the dependencies until all modules
            // have been created.
            foreach (var asset in bundle.Assets)
            {
                originalSources[asset] = Read(asset);
            }
            dependencies = new Lazy<IAmdModule[]>(ParseDependencies);
            ParseExports();
            Export = new ObjectExport(PathAsModuleIdentifier, allExports);
        }

        string Read(IAsset asset)
        {
            using (var reader = new StreamReader(asset.OpenStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public string Path { get; private set; }

        public IAmdModule[] Dependencies
        {
            get { return dependencies.Value; }
        }

        public IExport Export { get; private set; }

        IAmdModule[] ParseDependencies()
        {
            return bundle
                .Assets
                .Where(a => originalSources.ContainsKey(a))
                .Select(a => new { source = originalSources[a], path = a.Path })
                .SelectMany(x => ScriptReferenceParser.ParseReferences(x.source, x.path))
                .Distinct()
                .Select(resolveReferencePathIntoAmdModule)
                .Distinct()
                .ToArray();
        }

        void ParseExports()
        {
            foreach (var asset in bundle.Assets)
            {
                var exports = GlobalJavaScriptVariables(asset).ToArray();
                exportsByAsset[asset] = exports;
                allExports.AddRange(exports);
            }
        }

        string PathAsModuleIdentifier
        {
            get { return Path.Replace('/', '_'); }
        }

        IEnumerable<string> GlobalJavaScriptVariables(IAsset asset)
        {
            using (var reader = new StreamReader(asset.OpenStream()))
            {
                var javaScript = reader.ReadToEnd();
                return GlobalJavaScriptVariableParser.GetVariables(javaScript);
            }
        }

        public IEnumerable<string> GetExportsFromAsset(IAsset asset)
        {
            return exportsByAsset[asset];
        }

        public IEnumerable<string> GetExportsDefinedBeforeAsset(IAsset asset)
        {
            var found = false;
            var exports = new List<string>();
            var visitor = new BundleVisitor(a =>
            {
                if (found) return;
                if (a == asset)
                {
                    found = true;
                }
                else
                {
                    exports.AddRange(exportsByAsset[a]);
                }
            });
            bundle.Accept(visitor);
            return exports;
        } 
    }
}