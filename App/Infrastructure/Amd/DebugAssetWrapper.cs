using System.Collections.Generic;
using System.Linq;
using Cassette;

namespace App.Infrastructure.Amd
{
    public class DebugAssetWrapper : StringAssetTransformer
    {
        readonly DebugAmdModuleFromBundle module;

        public DebugAssetWrapper(DebugAmdModuleFromBundle module)
        {
            this.module = module;
        }

        public static string Wrap(string source, IEnumerable<string> exportedVariables, IEnumerable<string> thisModuleVariables, IEnumerable<IExport> imports)
        {
            var parameters = string.Join(",", imports.Select(i => i.Identifier));

            var sameModuleAliases =
                from variable in thisModuleVariables
                select string.Format("var {0}=this.{0};", variable);
            var otherModuleAliases =
                from import in imports
                from alias in import.Aliases
                select string.Format("var {0}={1}.{0};", alias, import.Identifier);
            var aliases = string.Join("", sameModuleAliases.Concat(otherModuleAliases));

            var start = string.Format("define([],function(){{return function({0}){{{1}", parameters, aliases);

            var exports = string.Join("", exportedVariables.Select(e => string.Format("this.{0}={0};", e)));
            var end = exports + "}});";

            return start + source + end;
        }

        protected override string Transform(string source, IAsset asset)
        {
            var exports = module.GetExportsFromAsset(asset);
            var localImports = module.GetExportsDefinedBeforeAsset(asset);
            return Wrap(source, exports, localImports, module.Dependencies.Select(d => d.Export));
        }
    }
}