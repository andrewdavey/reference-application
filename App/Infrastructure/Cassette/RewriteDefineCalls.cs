using System;
using System.Text.RegularExpressions;
using Cassette;
using Microsoft.Ajax.Utilities;

namespace App.Infrastructure.Cassette
{
    class RewriteDefineCalls : StringAssetTransformer
    {
        protected override string Transform(string source, IAsset asset)
        {
            var parser = new JSParser(source);
            var ast = parser.Parse(new CodeSettings());

            var amdModuleName = Regex.Match(asset.Path, @"/([^/]*)\.js$").Groups[1].Value;
            ast.Accept(new Visitor(amdModuleName));
            var output = ast.ToCode();

            string shim;
            if (asset.TryGetMetaData("AmdShim", out shim))
            {
                output += CreateShimDefine(shim, amdModuleName);
            }

            return output;
        }

        string CreateShimDefine(string shim, string amdModuleName)
        {
            return string.Format(
                "{0};define('{1}',[],function(){{return {2}}});",
                Environment.NewLine,
                amdModuleName,
                shim
            );
        }

        class Visitor : TreeVisitor
        {
            readonly string amdModuleName;

            public Visitor(string amdModuleName)
            {
                this.amdModuleName = amdModuleName;
            }

            public override void Visit(CallNode node)
            {
                var lookup = node.Function as Lookup;
                if (lookup != null
                    && lookup.Name == "define"
                    && node.Arguments.Count > 0
                    && node.Arguments[0].FindPrimitiveType() != PrimitiveType.String)
                {
                    node.Arguments.Insert(0, new ConstantWrapper(amdModuleName, PrimitiveType.String, node.Context, node.Parser));
                }
                base.Visit(node);
            }
        }
    }

}