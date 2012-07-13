using System.IO;
using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    class RecordExportedVariables : IBundleProcessor<ScriptBundle>, IBundleVisitor
    {
        public void Process(ScriptBundle bundle)
        {
            bundle.Accept(this);
        }

        public void Visit(Bundle bundle)
        {
        }

        public void Visit(IAsset asset)
        {
            using (var reader = new StreamReader(asset.OpenStream()))
            {
                var source = reader.ReadToEnd();
                asset.RecordExportedVariables(source);
            }
        }
    }
}