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
            // Custom search for script and HTML templates.
            var fileSearch = new FileSearch
            {
                Pattern = "*.js;*.htm;*.html",
                SearchOption = SearchOption.AllDirectories
            };

            bundles.AddPerSubDirectory<ScriptBundle>(
                "Pages",
                fileSearch,
                b => b.EmbedHtmlTemplates().AmdModule()
            );
            bundles.AddPerSubDirectory<StylesheetBundle>("Pages");

            bundles.AddPerSubDirectory<ScriptBundle>("Modules", b => b.AmdModule());

            bundles.Add<ScriptBundle>(
                "Infrastructure/Scripts", 
                b => b.AmdModulePerAsset()
                      .AmdAlias("jquery.js", "$")
                      .AmdAlias("knockout.js", "ko")
                      .AmdShim("jquery.history.js", "History", "jquery")
            );
        }
    }
}