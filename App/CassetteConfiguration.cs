using System.IO;
using App.Infrastructure.Cassette;
using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace App
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            AddInfrastructureBundles(bundles);
            AddAppBundle(bundles);

            AddBundle("Application", bundles);
            AddBundle("Dashboard", bundles);
            AddBundle("Profile", bundles);
            AddBundlePerSubDirectory("Vehicles", bundles);

            AddBundle("Specs", bundles);
        }

        void AddBundle(string path, BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>(
                path,
                ScriptAndHtmlTemplateFileSearch(),
                b => b.EmbedHtmlTemplates().AmdModule()
            );
            bundles.Add<StylesheetBundle>(path);
        }


        void AddBundlePerSubDirectory(string path, BundleCollection bundles)
        {
            bundles.AddPerSubDirectory<ScriptBundle>(
                path,
                ScriptAndHtmlTemplateFileSearch(),
                b => b.EmbedHtmlTemplates().AmdModule()
            );
            bundles.AddPerSubDirectory<StylesheetBundle>(path);
        }

        void AddAppBundle(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>(
                "Infrastructure/Scripts/App",
                b => b.AmdModule()
            );
        }

        void AddInfrastructureBundles(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>(
                "Infrastructure/Scripts/Vendor",
                b => b.AmdModulePerAsset()
                      .AmdAlias("jquery.js", "$")
                      .AmdAlias("knockout.js", "ko")
                      .AmdAlias("moment.js", "moment") // TODO: Un-hack the define() call in moment.js!
                      .AmdShim("bootstrap/js/bootstrap.js", "bootstrap", "jquery") // TODO: fix this shim helper
                      .AmdShim("jquery.history.js", "History", "jquery") // TODO: fix this shim helper
            );
            bundles.AddPerSubDirectory<ScriptBundle>(
                "Infrastructure/Scripts/lang",
                b => b.AmdModule()
            );
            bundles.Add<StylesheetBundle>(
                "Infrastructure/Scripts/Vendor"
            );
        }

        /// <summary>
        /// Custom file search that finds all script and HTML templates.
        /// </summary>
        FileSearch ScriptAndHtmlTemplateFileSearch()
        {
            return new FileSearch
            {
                Pattern = "*.js;*.htm;*.html",
                SearchOption = SearchOption.AllDirectories
            };
        }
    }
}