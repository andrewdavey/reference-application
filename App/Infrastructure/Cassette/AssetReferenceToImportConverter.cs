using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;

namespace App.Infrastructure.Cassette
{
    public class AssetReferenceToImportConverter
    {
        readonly BundleCollection bundles;

        public AssetReferenceToImportConverter(BundleCollection bundles)
        {
            this.bundles = bundles;
        }

        public IEnumerable<Import> Convert(AssetReference assetReference)
        {
            switch (assetReference.Type)
            {
                case AssetReferenceType.SameBundle:
                    return ConvertSameBundle(assetReference);

                case AssetReferenceType.DifferentBundle:
                    return ConvertDifferentBundle(assetReference);

                default:
                    return Enumerable.Empty<Import>();
            }
        }

        IEnumerable<Import> ConvertSameBundle(AssetReference assetReference)
        {
            var asset = GetAsset(assetReference.ToPath);
            return from variable in asset.GetExportedVariables()
                   select Import.FromAssetInSameModule(variable);
        }

        IEnumerable<Import> ConvertDifferentBundle(AssetReference assetReference)
        {
            var tuple = GetAssetAndBundle(assetReference.ToPath);
            var asset = tuple.Item1;
            var bundle = tuple.Item2;

            string alias;
            if (asset.TryGetMetaData("AmdAlias", out alias))
            {
                yield return Import.FromAmdAsset(asset.Path.Substring(2), alias);
            }
            else
            {
                var variables = asset.GetExportedVariables();
                var modulePath = GetModulePathForAsset(bundle);
                foreach (var variable in variables)
                {
                    yield return Import.FromAssetInDifferentModule(modulePath, variable);
                }
            }
        }

        Tuple<IAsset, Bundle> GetAssetAndBundle(string assetPath)
        {
            var finder = new BundleAssetFinder(assetPath);
            bundles.Accept(finder);

            if (finder.FoundAsset == null) throw new ArgumentException("Cannot find asset " + assetPath);

            return Tuple.Create(finder.FoundAsset, finder.FoundBundle);
        }

        IAsset GetAsset(string assetPath)
        {
            return GetAssetAndBundle(assetPath).Item1;
        }

        string GetModulePathForAsset(Bundle bundle)
        {
            return bundle.Path.Substring(2);
        }
    }
}