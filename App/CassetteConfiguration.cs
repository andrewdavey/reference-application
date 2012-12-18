using System;
using System.IO;
using System.Linq;
using App.Infrastructure.Cassette;
using Cassette;
using Cassette.RequireJS;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace App
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        readonly CassetteSettings settings;
        BundleCollection bundles;

        public CassetteBundleConfiguration(CassetteSettings settings)
        {
            this.settings = settings;
        }

        public void Configure(BundleCollection bundles)
        {
            this.bundles = bundles;
            AddVendorBundles();
            AddSharedBundle();
            AddPageBundles();

            if (settings.IsDebuggingEnabled)
            {
                bundles.Add<ScriptBundle>("ClientSpecs");
            }

            InitRequireJsModules();
        }

        void InitRequireJsModules()
        {
            // Convert basic bundles into AMD modules for use with require.js.
            bundles.InitializeRequireJsModules("Client/Vendor/require.js", amd =>
            {
                amd.SetImportAlias("Client/Vendor/jquery.js", "$");
                amd.SetImportAlias("Client/Vendor/knockout.js", "ko");
                amd.SetImportAlias("Client/Vendor/jquery.history.js", "History");
                amd.SetModuleReturnExpression("Client/Vendor/jquery.history.js", "History");

                foreach (var module in amd)
                {
                    if (module.Asset.Path.EndsWith(".htm") || module.Asset.Path.EndsWith(".html"))
                    {
                        module.ModulePath += "-template";
                    }
                }
            });
        }

        void AddSharedBundle()
        {
            AddBundle("Shared");
            AddBundle("lang");
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
                b => b.EmbedHtmlTemplates()
            );
            bundles.Add<StylesheetBundle>(path);
        }

        void AddPageBundlePerSubDirectory(string path)
        {
            path = "Client/" + path;
            bundles.AddPerSubDirectory<ScriptBundle>(
                path,
                ScriptAndHtmlTemplateFileSearch(),
                b => b.EmbedHtmlTemplates()
            );
            bundles.AddPerSubDirectory<StylesheetBundle>(path);
        }

        void AddBundle(string path)
        {
            bundles.Add<ScriptBundle>("Client/" + path);
        }

        void AddVendorBundles()
        {
            bundles.Add<ScriptBundle>("Client/Vendor");
            bundles.AddPerSubDirectory<ScriptBundle>("Client/lang");
            bundles.Add<StylesheetBundle>("Client/Vendor");
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