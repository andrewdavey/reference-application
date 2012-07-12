using Cassette;

namespace App.Infrastructure.Cassette
{
    class IifeWrapper : StringAssetTransformer
    {
        protected override string Transform(string source, IAsset asset)
        {
            return "(function() {" + source + "\n}());";
        }
    }
}