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
        }

        public string Path { get; private set; }
        public IExport Export { get; private set; }
    }
}