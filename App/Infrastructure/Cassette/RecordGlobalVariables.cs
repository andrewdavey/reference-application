using System.Collections.Generic;
using System.IO;
using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    class RecordGlobalVariables : IBundleProcessor<ScriptBundle>, IBundleVisitor
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
                var variables = ParseGlobalVariables(source);
                asset.SetMetaData("ExportedVariables", variables);
            }
        }

        IEnumerable<string> ParseGlobalVariables(string source)
        {
            return GlobalJavaScriptVariableParser.GetVariables(source);
        }
    }
}