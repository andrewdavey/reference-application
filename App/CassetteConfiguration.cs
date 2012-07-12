using System.IO;
using App.Infrastructure.Cassette;
using Cassette;
using Cassette.Scripts;

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
                Pattern = "*.js;*.htm",
                SearchOption = SearchOption.AllDirectories
            };

            bundles.AddPerSubDirectory<ScriptBundle>(
                "Pages",
                fileSearch,
                b => b.EmbedHtmlTemplates().AmdModule()
            );
        }
    }
}