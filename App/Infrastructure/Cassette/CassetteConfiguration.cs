using System.IO;
using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace App.Infrastructure.Cassette
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
            AddPageBundle("Dashboard", bundles);
            AddPageBundle("Vehicles/NewVehiclePage", bundles);
            AddPageBundle("Vehicles/VehiclePage", bundles);
        }

        void AddPageBundle(string path, BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>(
                path,
                ScriptAndHtmlTemplateFileSearch(),
                b => b.EmbedHtmlTemplates().AmdModule()
            );
            bundles.AddPerSubDirectory<StylesheetBundle>(path);
        }

        void AddAppBundle(BundleCollection bundles)
        {
            bundles.AddPerSubDirectory<ScriptBundle>(
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
                      .AmdShim("jquery.history.js", "History", "jquery") // TODO: fix this shim helper
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