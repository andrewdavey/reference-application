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
                .SelectMany(m => m.PathMaps())
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

        public void AddVendorModulesPerAsset(Bundle bundle, Action<Func<string, VendorModuleConfiguration>> build)
        {
            var configurations = new Dictionary<string, VendorModuleConfiguration>();
            build(assetFilename =>
            {
                var configuration = new VendorModuleConfiguration(assetFilename);
                configurations[assetFilename] = configuration;
                return configuration;
            });

            foreach (var asset in bundle.Assets)
            {
                var filename = asset.Path.Split('/').Last();
                VendorModuleConfiguration config;
                if (configurations.TryGetValue(filename, out config))
                {
                    modules.Add(config.Build(asset));
                }
                else
                {
                    modules.Add(new VendorAmdModule(asset, filename.Substring(0, filename.Length - 3).Replace('.', '_')));                    
                }
            }
        }

        public class VendorModuleConfiguration
        {
            readonly string assetFilename;
            string identifier;
            bool shim;
            string shimExports;
            string[] dependencies;

            public VendorModuleConfiguration(string assetFilename)
            {
                this.assetFilename = assetFilename;
            }

            public string AssetFilename
            {
                get { return assetFilename; }
            }

            public VendorModuleConfiguration Identifier(string identifier)
            {
                this.identifier = identifier;
                return this;
            }

            public VendorModuleConfiguration Shim(string exports = null)
            {
                shim = true;
                shimExports = exports;
                return this;
            }

            public VendorModuleConfiguration DependsOn(params string[] dependencies)
            {
                this.dependencies = dependencies;
                return this;
            }

            public VendorAmdModule Build(IAsset asset)
            {
                var module = new VendorAmdModule(asset, identifier);
                if (shim) module.Shim(shimExports, dependencies ?? new string[0]);
                return module;
            }
        }

        IAmdModule ResolveModuleFromPath(string bundleOrAssetPath)
        {
            var module = modules.FirstOrDefault(m => m.ContainsPath(bundleOrAssetPath));
            if (module != null) return module;

            throw new ArgumentException("Cannot find AMD module for the path \"" + bundleOrAssetPath + "\".");
        }
    }
}