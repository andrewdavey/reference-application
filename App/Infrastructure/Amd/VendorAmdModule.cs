using Cassette;

namespace App.Infrastructure.Amd
{
    public class VendorAmdModule : IAmdModule
    {
        readonly IAsset asset;

        public VendorAmdModule(IAsset asset, string identifier)
        {
            this.asset = asset;

            Path = asset.Path
                .Substring(0, asset.Path.LastIndexOf('.'))
                .TrimStart('~', '/');
            Export = new SingleValueExport(identifier);

            asset.AddAssetTransformer(new RewriteDefineCalls(Path));
        }

        public string Path { get; private set; }
        
        public IExport Export { get; private set; }

        public bool ContainsPath(string path)
        {
            if (path.EndsWith(".js")) path = path.Substring(0, path.Length - 3);
            return path.TrimStart('~', '/') == Path;
        }

        public void Shim(string shimExports, string[] dependencies)
        {
            asset.AddAssetTransformer(new ShimVendorModule(Path, dependencies, shimExports));
        }
    }
}