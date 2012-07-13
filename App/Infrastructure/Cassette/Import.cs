using System.Collections.Generic;
using System.Linq;

namespace App.Infrastructure.Cassette
{
    public abstract class Import
    {
        public static Import FromAssetInSameModule(string variableName)
        {
            return new AssetInSameModule(variableName);
        }

        public static Import FromAssetInDifferentModule(string modulePath, string variableName)
        {
            return new AssetInDifferentModule(modulePath, variableName);
        }

        public static Import FromAmdAsset(string modulePath, string variableName)
        {
            return new AmdAsset(modulePath, variableName);
        }

        public static IEnumerable<string> GetAllModulePaths(IEnumerable<Import> imports)
        {
            return imports.Where(i => i.ModulePath != null).Select(i => i.ModulePath);
        }

        public abstract string CreateAssignment(string moduleVariable, string dependenciesVariable);
        protected abstract string ModulePath { get; }

        class AssetInSameModule : Import
        {
            readonly string variableName;

            public AssetInSameModule(string variableName)
            {
                this.variableName = variableName;
            }

            public override string CreateAssignment(string moduleVariable, string dependenciesVariable)
            {
                // e.g. var foo = module.foo;
                return variableName + " = " + moduleVariable + "." + variableName;
            }

            protected override string ModulePath
            {
                get { return null; }
            }
        }

        class AssetInDifferentModule : Import
        {
            readonly string modulePath;
            readonly string variableName;

            public AssetInDifferentModule(string modulePath, string variableName)
            {
                this.modulePath = modulePath;
                this.variableName = variableName;
            }

            public override string CreateAssignment(string moduleVariable, string dependenciesVariable)
            {
                // e.g. var foo = deps['other'].foo;
                return variableName + " = " + dependenciesVariable + "['" + modulePath + "']." + variableName;
            }

            protected override string ModulePath
            {
                get { return modulePath; }
            }
        }

        class AmdAsset : Import
        {
            readonly string modulePath;
            readonly string variableName;

            public AmdAsset(string modulePath, string variableName)
            {
                this.modulePath = modulePath;
                this.variableName = variableName;
            }

            public override string CreateAssignment(string moduleVariable, string dependenciesVariable)
            {
                // e.g. var $ = deps["libs/jquery"];
                return variableName + " = " + dependenciesVariable + "['" + modulePath + "']";
            }

            protected override string ModulePath
            {
                get { return modulePath; }
            }
        }
    }
}