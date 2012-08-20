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
 
        public AmdModuleFromBundle(ScriptBundle bundle, Func<string, IAmdModule> resolveReferencePathIntoAmdModule)
        {
            this.bundle = bundle;
            this.resolveReferencePathIntoAmdModule = resolveReferencePathIntoAmdModule;
            Path = bundle.Path.TrimStart('~', '/');
            // Dependencies must be lazy because resolveReferencePathIntoAmdModule 
            // may need to return a module that hasn't yet been created.
            // Lazy means we can avoid parsing the dependencies until all modules
            // have been created.
            dependencies = new Lazy<IAmdModule[]>(ParseDependencies);
            Export = new ObjectExport(GlobalJavaScriptVariables());
        }

        public string Path { get; private set; }

        public IAmdModule[] Dependencies
        {
            get { return dependencies.Value; }
        }

        public object Export { get; private set; }

        IAmdModule[] ParseDependencies()
        {
            return bundle
                .Assets
                .SelectMany(ScriptReferenceParser.ParseReferences)
                .Distinct()
                .Select(resolveReferencePathIntoAmdModule)
                .Distinct()
                .ToArray();
        }

        IEnumerable<string> GlobalJavaScriptVariables()
        {
            return bundle.Assets.SelectMany(GlobalJavaScriptVariables);
        }

        IEnumerable<string> GlobalJavaScriptVariables(IAsset asset)
        {
            using (var reader = new StreamReader(asset.OpenStream()))
            {
                var javaScript = reader.ReadToEnd();
                return GlobalJavaScriptVariableParser.GetVariables(javaScript);
            }
        }
    }

    public class ObjectExport
    {
        public ObjectExport(IEnumerable<string> identifiers)
        {
            Identifiers = identifiers;
        }

        public IEnumerable<string> Identifiers { get; private set; }
    }
}