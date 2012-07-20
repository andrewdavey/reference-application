using System.Collections.Generic;
using System.Linq;
using Cassette;
using Cassette.Stylesheets;
using Newtonsoft.Json;

namespace App.Infrastructure.Cassette
{
    public class StylesheetPathProvider : IStartUpTask, IBundleVisitor
    {
        public static string PathMapJson { get; private set; }

        readonly BundleCollection bundles;
        readonly IUrlGenerator urlGenerator;
        readonly bool isDebuggingEnabled;
        readonly Dictionary<string, IEnumerable<string>> pathMap = new Dictionary<string, IEnumerable<string>>();
        List<string> currentPathList;

        public StylesheetPathProvider(BundleCollection bundles, IUrlGenerator urlGenerator, CassetteSettings settings)
        {
            this.bundles = bundles;
            this.urlGenerator = urlGenerator;
            isDebuggingEnabled = settings.IsDebuggingEnabled;
        }

        public void Start()
        {
            bundles.Changed += BundlesOnChanged;
        }

        void BundlesOnChanged(object sender, BundleCollectionChangedEventArgs bundleCollectionChangedEventArgs)
        {
            var stylesheets = bundleCollectionChangedEventArgs.Bundles.OfType<StylesheetBundle>();
            lock (pathMap)
            {
                pathMap.Clear();
                stylesheets.Accept(this);
                PathMapJson = JsonConvert.SerializeObject(pathMap);
            }
        }

        public void Visit(Bundle bundle)
        {
            var path = bundle.Path.Substring(2);
            if (isDebuggingEnabled)
            {
                pathMap[path] = currentPathList = new List<string>();
            }
            else
            {
                var url = urlGenerator.CreateBundleUrl(bundle);
                pathMap[path] = new[] {url};
            }
        }

        public void Visit(IAsset asset)
        {
            if (isDebuggingEnabled)
            {
                currentPathList.Add(urlGenerator.CreateAssetUrl(asset));
            }
        }
    }
}