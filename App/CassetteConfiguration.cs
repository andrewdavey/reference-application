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
                b =>
                {
                    amdModuleCollection.AddVendorModulesPerAsset(b, new Dictionary<string,string>
                    {
                        {"jquery.js", "$"},
                        {"knockout.js", "ko"},
                        {"moment.js", "moment"}
                    });
                    
                    /*
                        .AmdAlias("jquery.js", "$")
                        .AmdAlias("knockout.js", "ko")
                        .AmdAlias("moment.js", "moment") // TODO: Un-hack the define() call in moment.js!
                        .AmdShim("bootstrap/js/bootstrap.js", "bootstrap", "jquery") // TODO: fix this shim helper
                        .AmdAlias("bootstrap/js/datepicker.js", "datepicker") // TODO: fix this shim helper
                        .AmdShim("jquery.history.js", "History", "jquery");// TODO: fix this shim helper
                     */
                }
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