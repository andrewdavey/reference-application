using Cassette;

namespace App.Infrastructure.Cassette
{
    class PrependNamespace : StringAssetTransformer
    {
        private readonly string scriptNamespace;

        public PrependNamespace(Bundle bundle)
        {
            scriptNamespace = bundle.ScriptNamespace();
        }

        protected override string Transform(string source, IAsset asset)
        {
            var initRoot = "if (typeof Pages==='undefined'){Pages={}}";
            var initNamespace = "if (!" + scriptNamespace + ")" + scriptNamespace + "={templates:{}};";
            return initRoot + initNamespace + source;
        }
    }
}