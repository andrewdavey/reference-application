using Cassette;

namespace App.Infrastructure.Amd
{
    public class VendorAmdModule : IAmdModule
    {
        public VendorAmdModule(IAsset asset, string identifier)
        {
            Path = asset.Path
                .Substring(0, asset.Path.LastIndexOf('.'))
                .TrimStart('~', '/');
            Export = new SingleValueExport(identifier);

            asset.AddAssetTransformer(new RewriteDefineCalls());
        }

        public string Path { get; private set; }
        
        public IExport Export { get; private set; }

        public bool ContainsPath(string path)
        {
            if (path.EndsWith(".js")) path = path.Substring(0, path.Length - 3);
            return path.TrimStart('~', '/') == Path;
        }
    }
}