using System.Collections.Generic;
using System.Linq;
using Cassette;
using Cassette.Stylesheets;

namespace App.Infrastructure
{
    public class PageHelper : IStartUpTask
    {
        readonly BundleCollection bundles;
        readonly IUrlGenerator urlGenerator;
        readonly CassetteSettings settings;

        public PageHelper(BundleCollection bundles, IUrlGenerator urlGenerator, CassetteSettings settings)
        {
            this.bundles = bundles;
            this.urlGenerator = urlGenerator;
            this.settings = settings;
        }

        public void Start()
        {
            Page.Helper = this;
        }

        public IEnumerable<string> GetStylesheetUrls(string path)
        {
            using (bundles.GetReadLock())
            {
                var bundle = bundles.FindBundlesContainingPath(path).OfType<StylesheetBundle>().FirstOrDefault();
                if (bundle == null) return Enumerable.Empty<string>();
                if (settings.IsDebuggingEnabled)
                {
                    return bundle.Assets.Select(urlGenerator.CreateAssetUrl);
                }
                else
                {
                    return new[] {urlGenerator.CreateBundleUrl(bundle)};
                }
            }
        }
    }
}