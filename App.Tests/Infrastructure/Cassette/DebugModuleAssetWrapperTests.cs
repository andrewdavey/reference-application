using System.Collections.Generic;
using Xunit;

namespace App.Infrastructure.Cassette
{
    public class DebugModuleAssetWrapperTests
    {
        readonly DebugModuleAssetWrapper wrapper = new DebugModuleAssetWrapper();

        const string ModulePath = "module/path";
        readonly List<Import> imports = new List<Import>();
        readonly List<string> exports = new List<string>();
        
        [Fact]
        public void WrapAddsModuleInitializationFunction()
        {
            var wrapped = wrapper.Wrap("OriginalSourceCode", ModulePath, imports, exports);

            Assert.Equal(@"debugModules['module/path'].push(function(module,deps){OriginalSourceCode
});", wrapped);
        }

        [Fact]
        public void WrapAddsModuleInitializationFunctionThatAliasesImports()
        {
            imports.Add(Import.FromAmdAsset("libs/jquery", "$"));
            var wrapped = wrapper.Wrap("OriginalSourceCode", ModulePath, imports, exports);

            Assert.Equal(@"debugModules['module/path'].push(function(module,deps){var $ = deps['libs/jquery'];OriginalSourceCode
});", wrapped);
        }

        [Fact]
        public void WrapAddsModuleInitializationFunctionThatDefinesModuleExports()
        {
            exports.AddRange(new[]{"export1", "export2"});
            var wrapped = wrapper.Wrap("OriginalSourceCode", ModulePath, imports, exports);

            Assert.Equal(@"debugModules['module/path'].push(function(module,deps){OriginalSourceCode
module.export1 = export1;
module.export2 = export2;});", wrapped);
        }
    }
}