using System.Collections.Generic;
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

        public CassetteBundleConfiguration(AmdModuleCollection amdModuleCollection)
        {
            this.amdModuleCollection = amdModuleCollection;
            AmdModuleCollection.Instance = amdModuleCollection; // TODO: Clean up this hacky global!
        }

        public void Configure(BundleCollection bundles)
        {
            amdModuleCollection.Clear();
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
                b =>
                {
                    b.EmbedHtmlTemplates();
                    amdModuleCollection.AddModuleFromBundle(b);
                }
            );
            bundles.Add<StylesheetBundle>(path);
        }


        void AddBundlePerSubDirectory(string path, BundleCollection bundles)
        {
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

        void AddAppBundle(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>(
                "Infrastructure/Scripts/App",
                b => amdModuleCollection.AddModuleFromBundle(b));
        }

        void AddInfrastructureBundles(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>(
                "Infrastructure/Scripts/Vendor",
                b => amdModuleCollection.AddVendorModulesPerAsset(b, config =>
                {
                    config("jquery.js").Identifier("$");
                    config("knockout.js").Identifier("ko");
                    config("moment.js").Identifier("moment");// TODO: Un-hack the define() call in moment.js!
                    config("jquery.history.js").Identifier("History").Shim("History").DependsOn("Infrastructure/Scripts/Vendor/jquery");
                    config("bootstrap.js").Identifier("bootstrap").Shim().DependsOn("Infrastructure/Scripts/Vendor/jquery");
                    config("datepicker.js").Identifier("datepicker").Shim().DependsOn("Infrastructure/Scripts/Vendor/jquery");
                })
            );
            bundles.AddPerSubDirectory<ScriptBundle>(
                "Infrastructure/Scripts/lang",
                b => amdModuleCollection.AddModuleFromBundle(b)
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