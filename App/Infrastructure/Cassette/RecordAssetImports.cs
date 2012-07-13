using System.Collections.Generic;
using Cassette;

namespace App.Infrastructure.Cassette
{
    public static class ImportHelpers
    {
        static BundleCollection bundles;

        public class StartUp : IStartUpTask
        {
            readonly BundleCollection bundles;

            public StartUp(BundleCollection bundles)
            {
                this.bundles = bundles;
            }

            public void Start()
            {
                ImportHelpers.bundles = bundles;
            }
        }

        public static IEnumerable<Import> GetImports(this IAsset asset)
        {
            var converter = new AssetReferenceToImportConverter(bundles);
            var imports = new List<Import>();
            foreach (var assetReference in asset.References)
            {
                imports.AddRange(converter.Convert(assetReference));
            }
            return imports;
        } 
    }
}