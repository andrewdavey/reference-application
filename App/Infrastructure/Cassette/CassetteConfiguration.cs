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
            AddBundle("Dashboard", bundles);
            AddBundle("Profile", bundles);
            AddBundle("Vehicles/List", bundles);
            AddBundle("Vehicles/NewVehiclePage", bundles);
            AddBundle("Vehicles/VehiclePage", bundles);
            AddBundle("Vehicles/FillUpsPage", bundles);

            AddBundle("Specs", bundles);
        }

        void AddBundle(string path, BundleCollection bundles)
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