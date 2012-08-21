using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;
using Cassette.Scripts;

namespace App.Infrastructure.Amd
{
    public class AmdModuleCollection
    {
        public static AmdModuleCollection Instance;

        readonly IUrlGenerator urlGenerator;
        readonly CassetteSettings settings;
        readonly List<IAmdModule> modules;
        readonly List<Bundle> bundles;
 
        public AmdModuleCollection(IUrlGenerator urlGenerator, CassetteSettings settings)
        {
            this.urlGenerator = urlGenerator;
            this.settings = settings;
            modules = new List<IAmdModule>();
            bundles = new List<Bundle>();
        }

        public Require Require
        {
            get
            {
                if (settings.IsDebuggingEnabled)
                {
                    return new Require
                    {
                        Paths = DebugPaths()
                    };
                }
                else
                {
                    return new Require
                    {
                        Paths = ProductionPaths()
                    };
                }
            }
        }

        Dictionary<string, string> DebugPaths()
        {
            return modules
                .OfType<AmdModuleFromBundle>()
                .SelectMany(m => m.PathMaps(urlGenerator))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        Dictionary<string, string> ProductionPaths()
        {
            return modules
                .OfType<AmdModuleFromBundle>()
                .ToDictionary(
                    m => m.Path,
                    m => urlGenerator.CreateBundleUrl(m.Bundle)
                );
        }

        public void AddModuleFromBundle(ScriptBundle bundle)
        {
            bundles.Add(bundle);
            if (settings.IsDebuggingEnabled)
            {
                modules.Add(new DebugAmdModuleFromBundle(bundle, ResolveModuleFromPath, urlGenerator));
            }
            else
            {
                modules.Add(new ProductionAmdModuleFromBundle(bundle, ResolveModuleFromPath));
            }
        }

        public void AddVendorModulesPerAsset(Bundle bundle, Dictionary<string, string> identifiers)
        {
            foreach (var asset in bundle.Assets)
            {
                string identifier;
                var filename = asset.Path.Split('/').Last();
                if (identifiers.TryGetValue(filename, out identifier))
                {
                    modules.Add(new VendorAmdModule(asset, identifier));
                }
                else
                {
                    modules.Add(new VendorAmdModule(asset, filename.Substring(0, filename.Length - 3).Replace('.', '_')));
                }
            }
            // TODO: Add shims
        }

        IAmdModule ResolveModuleFromPath(string bundleOrAssetPath)
        {
            var module = modules.FirstOrDefault(m => m.ContainsPath(bundleOrAssetPath));
            if (module != null) return module;

            throw new ArgumentException("Cannot find AMD module for the path \"" + bundleOrAssetPath + "\".");
        }
    }
}