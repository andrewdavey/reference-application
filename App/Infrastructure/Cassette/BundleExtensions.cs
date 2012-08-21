using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    static class BundleExtensions
    {
        public static ScriptBundle EmbedHtmlTemplates(this ScriptBundle bundle)
        {
            bundle.Pipeline.Insert<ConvertAllHtmlTemplatesToScript>(0);
            return bundle;
        }
    }
}