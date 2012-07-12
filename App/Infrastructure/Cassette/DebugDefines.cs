using Cassette;

namespace App.Infrastructure.Cassette
{
    public class DebugDefines : IStartUpTask
    {
        readonly BundleCollection bundles;
        private readonly IUrlGenerator urlGenerator;

        public static string Shims { get; private set; }

        public DebugDefines(BundleCollection bundles, IUrlGenerator urlGenerator)
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
            var shimBuilder = new DebugDefineBuilder(urlGenerator);
            bundleCollectionChangedEventArgs.Bundles.Accept(shimBuilder);
            Shims = shimBuilder.ToString();
        }
    }
}