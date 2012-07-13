using System.Linq;
using Cassette;

namespace App.Infrastructure.Cassette
{
    public class DebugModuleDefinitionsProvider : IStartUpTask
    {
        public static string ModuleDefinitions { get; private set; }

        readonly BundleCollection bundles;
        readonly IUrlGenerator urlGenerator;

        public DebugModuleDefinitionsProvider(BundleCollection bundles, IUrlGenerator urlGenerator)
        {
            this.bundles = bundles;
            this.urlGenerator = urlGenerator;
        }

        public void Start()
        {
            bundles.Changed += BundlesOnChanged;
        }

        private void BundlesOnChanged(object sender, BundleCollectionChangedEventArgs bundleCollectionChangedEventArgs)
        {
            var bundles = bundleCollectionChangedEventArgs
                .Bundles
                .Where(b => !b.GetMetaDataOrDefault("AmdModulePerAsset", false));

            var builder = new DebugModuleCollectionBuilder(urlGenerator);
            bundles.Accept(builder);

            ModuleDefinitions = builder.Build();
        }
    }
}