using Cassette;
using Newtonsoft.Json;

namespace App.Infrastructure.Amd
{
    public class ShimVendorModule : StringAssetTransformer
    {
        readonly string path;
        readonly string[] dependencies;
        readonly string export;

        public ShimVendorModule(string path, string[] dependencies, string export)
        {
            this.path = path;
            this.dependencies = dependencies;
            this.export = export;
        }

        protected override string Transform(string source, IAsset asset)
        {
            // TODO: Generate function parameters for the dependencies.
            return string.Format(
                "define({0},{1},function(){{{2}\nreturn {3};}});",
                JsonConvert.SerializeObject(path),
                JsonConvert.SerializeObject(dependencies),
                source,
                export
            );
        }
    }
}