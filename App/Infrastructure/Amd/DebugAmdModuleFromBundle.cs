using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;
using Newtonsoft.Json;

namespace App.Infrastructure.Amd
{
    public class DebugAmdModuleFromBundle : AmdModuleFromBundle, IBundleProcessor<ScriptBundle>
    {
        readonly IEnumerable<string> assetPaths;

        public DebugAmdModuleFromBundle(ScriptBundle bundle, Func<string, IAmdModule> resolveReferencePathIntoAmdModule)
            : base(bundle, resolveReferencePathIntoAmdModule)
        {
            assetPaths = bundle.Assets.Select(a => a.Path.TrimStart('~', '/'));
            bundle.Pipeline.Insert(0, this);
        }

        public string DefinitionShim
        {
            get
            {
                var path = JsonConvert.SerializeObject(Path);

                var dependenciesAndAssets = JsonConvert.SerializeObject(
                    Dependencies.Select(d => d.Path).Concat(assetPaths)
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
        }

        public void Process(ScriptBundle bundle)
        {
            AddDebugAssetWrapperToAssets(bundle);
            AddShimAsset(bundle);
        }

        void AddShimAsset(ScriptBundle bundle)
        {
            bundle.Assets.Add(new ShimAsset(this));
        }

        void AddDebugAssetWrapperToAssets(ScriptBundle bundle)
        {
            foreach (var asset in bundle.Assets)
            {
                asset.AddAssetTransformer(new DebugAssetWrapper(this));
            }
        }

        class ShimAsset : IAsset
        {
            readonly DebugAmdModuleFromBundle module;

            public ShimAsset(DebugAmdModuleFromBundle module)
            {
                this.module = module;
            }

            public void Accept(IBundleVisitor visitor)
            {
                visitor.Visit(this);
            }

            public void AddAssetTransformer(IAssetTransformer transformer)
            {
                throw new NotImplementedException();
            }

            public void AddReference(string assetRelativePath, int lineNumber)
            {
                throw new NotImplementedException();
            }

            public void AddRawFileReference(string relativeFilename)
            {
                throw new NotImplementedException();
            }

            public Stream OpenStream()
            {
                var stream = new MemoryStream();
                var bytes = Encoding.UTF8.GetBytes(module.DefinitionShim);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                return stream;
            }

            public Type AssetCacheValidatorType { get; private set; }

            public byte[] Hash
            {
                get
                {
                    using (var sha1 = SHA1.Create())
                    {
                        return sha1.ComputeHash(Encoding.UTF8.GetBytes(module.DefinitionShim));
                    }
                }
            }

            public string Path
            {
                get { return "~/" + module.Path + "/debug-shim.js"; }
            }

            public IEnumerable<AssetReference> References
            {
                get { yield break; }
            }
        }
    }
}