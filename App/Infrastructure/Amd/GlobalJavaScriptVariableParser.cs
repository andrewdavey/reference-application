using System.Collections.Generic;
using Microsoft.Ajax.Utilities;

namespace App.Infrastructure.Amd
{
    class GlobalJavaScriptVariableParser : TreeVisitor
    {
        public static IEnumerable<string> GetVariables(string source)
        {
            var parser = new JSParser(source);
            var sourceTree = parser.Parse(new CodeSettings());
            var visitor = new GlobalJavaScriptVariableParser();
            sourceTree.Accept(visitor);
            return visitor.globalVariables;
        }

        readonly List<string> globalVariables = new List<string>();

        public override void Visit(VariableDeclaration node)
        {
            base.Visit(node);

            if (node.EnclosingScope is GlobalScope)
            {
                globalVariables.Add(node.Identifier);
            }
        }
    }
}