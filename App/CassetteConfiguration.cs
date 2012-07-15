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
            AddPageBundles(bundles);
            AddModuleBundles(bundles);
            AddInfrastructureBundles(bundles);
        }

        void AddPageBundles(BundleCollection bundles)
        {
            bundles.AddPerSubDirectory<ScriptBundle>(
                "Pages",
                ScriptAndHtmlTemplateFileSearch(),
                b => b.EmbedHtmlTemplates().AmdModule()
            );
            bundles.AddPerSubDirectory<StylesheetBundle>("Pages");
        }

        void AddModuleBundles(BundleCollection bundles)
        {
            bundles.AddPerSubDirectory<ScriptBundle>(
                "Modules",
                b => b.AmdModule()
            );
        }

        void AddInfrastructureBundles(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>(
                "Infrastructure/Scripts",
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