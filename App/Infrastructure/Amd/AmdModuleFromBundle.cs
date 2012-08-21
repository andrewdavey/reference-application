using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Infrastructure.Cassette;
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
            foreach (var asset in bundle.Assets)
            {
                originalSources[asset] = Read(asset);
            }
            // Dependencies must be lazy because resolveReferencePathIntoAmdModule 
            // may need to return a module that hasn't yet been created.
            // Lazy means we can avoid parsing the dependencies until all modules
            // have been created.
            dependencies = new Lazy<IAmdModule[]>(ParseDependencies);
            // TODO: Remove the following hack
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

        public bool ContainsPath(string path)
        {
            if (Bundle.Path == path) return true;

            var foundAsset = false;
            var visitor = new BundleVisitor
            {
                VisitAsset = a =>
                {
                    if (a.Path == path)
                    {
                        foundAsset = true;
                    }
                }
            };
            Bundle.Accept(visitor);

            return foundAsset;
        }

        IAmdModule[] ParseDependencies()
        {
            var existingReferences = Bundle
                .Assets
                .SelectMany(a => a.References)
                .Where(r => r.Type == AssetReferenceType.DifferentBundle)
                .Select(r => r.ToPath);

            return Bundle
                .Assets
                .Where(a => originalSources.ContainsKey(a))
                .Select(a => new {source = originalSources[a], path = a.Path})
                .SelectMany(x => ScriptReferenceParser.ParseReferences(x.source, x.path))
                .Concat(existingReferences)
                .Distinct()
                .Select(resolveReferencePathIntoAmdModule)
                .Distinct()
                .Except(new[] {this})
                .ToArray();
        }

        void ParseExports()
        {
            foreach (var asset in Bundle.Assets)
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

        protected ScriptBundle Bundle
        {
            get { return bundle; }
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
            var visitor = new BundleVisitor
            {
                VisitAsset = a =>
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
                }
            };
            Bundle.Accept(visitor);
            return exports;
        }

        public virtual IEnumerable<KeyValuePair<string, string>> PathMaps()
        {
            yield break;
        }
    }
}