using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cassette;

namespace App.Infrastructure.Amd
{
    class ShimAsset : IAsset
    {
        readonly DebugAmdModuleFromBundle module;
        readonly IUrlGenerator urlGenerator;

        public ShimAsset(DebugAmdModuleFromBundle module, IUrlGenerator urlGenerator)
        {
            this.module = module;
            this.urlGenerator = urlGenerator;
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
            var bytes = Encoding.UTF8.GetBytes(module.DefinitionShim());
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
                using (var stream = OpenStream())
                {
                    return sha1.ComputeHash(stream);
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