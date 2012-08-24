using System.IO;
using App.Infrastructure.Amd;
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
        readonly AmdModuleCollection amdModuleCollection;
        BundleCollection bundles;

        public CassetteBundleConfiguration(AmdModuleCollection amdModuleCollection)
        {
            this.amdModuleCollection = amdModuleCollection;
            AmdModuleCollection.Instance = amdModuleCollection; // TODO: Clean up this hacky global!
        }

        public void Configure(BundleCollection bundles)
        {
            this.bundles = bundles;
            amdModuleCollection.Clear(); // TODO: change AmdModuleCollection to be transient to avoid needing to clear singleton?

            AddVendorBundles();
            AddSharedBundle();
            AddPageBundles();
            // TODO: Only do this when debugging.
            AddPageBundle("Specs");
        }

        void AddSharedBundle()
        {
            AddBundle("Shared");
        }

        void AddPageBundles()
        {
            AddPageBundle("AppFrame");
            AddPageBundle("Dashboard");
            AddPageBundle("Profile");
            AddPageBundlePerSubDirectory("Vehicles"); // Details, FillUps, List, etc.
        }

        void AddPageBundle(string path)
        {
            path = "Client/" + path;
            bundles.Add<ScriptBundle>(
                path,
                ScriptAndHtmlTemplateFileSearch(),
                b =>
                {
                    b.EmbedHtmlTemplates();
                    amdModuleCollection.AddModuleFromBundle(b);
                }
            );
            bundles.Add<StylesheetBundle>(path);
        }


        void AddPageBundlePerSubDirectory(string path)
        {
            path = "Client/" + path;
            bundles.AddPerSubDirectory<ScriptBundle>(
                path,
                ScriptAndHtmlTemplateFileSearch(),
                b =>
                {
                    b.EmbedHtmlTemplates();
                    amdModuleCollection.AddModuleFromBundle(b);
                }
            );
            bundles.AddPerSubDirectory<StylesheetBundle>(path);
        }

        void AddBundle(string path)
        {
            bundles.Add<ScriptBundle>(
                "Client/" + path,
                b => amdModuleCollection.AddModuleFromBundle(b));
        }

        void AddVendorBundles()
        {
            bundles.Add<ScriptBundle>(
                "Client/Vendor",
                b => amdModuleCollection.AddVendorModulesPerAsset(b, config =>
                {
                    config("jquery.js").Identifier("$");
                    config("knockout.js").Identifier("ko");
                    config("moment.js").Identifier("moment");// TODO: Un-hack the define() call in moment.js!
                    config("jquery.history.js").Identifier("History").Shim("History").DependsOn("Client/Vendor/jquery");
                    config("bootstrap.js").Identifier("bootstrap").Shim().DependsOn("Client/Vendor/jquery");
                    config("datepicker.js").Identifier("datepicker").Shim().DependsOn("Client/Vendor/jquery");
                })
            );
            bundles.AddPerSubDirectory<ScriptBundle>(
                "Client/lang",
                b => amdModuleCollection.AddModuleFromBundle(b)
            );
            bundles.Add<StylesheetBundle>(
                "Client/Vendor"
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