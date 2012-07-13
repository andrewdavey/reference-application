using Cassette;
using Cassette.BundleProcessing;

namespace App.Infrastructure.Cassette
{
    class AddTransformerToAssets<TTransformer, TBundle> : AddTransformerToAssets<TBundle>
        where TTransformer : IAssetTransformer, new()
        where TBundle : Bundle
    {
        protected override IAssetTransformer CreateAssetTransformer(TBundle bundle)
        {
            return new TTransformer();
        }
    }
}