using Xunit;

namespace App.Infrastructure.Cassette
{
    public class ImportTests
    {
        [Fact]
        public void ImportFromAssetInSameModuleGetsVariableFromModule()
        {
            var import = Import.FromAssetInSameModule("x");
            var script = import.CreateAssignment("module", "unused");
            Assert.Equal("x = module.x", script);
        }

        [Fact]
        public void ImportFromAssetInDifferentModuleGetsVariableFromDependencies()
        {
            var import = Import.FromAssetInDifferentModule("module/path", "x");
            var script = import.CreateAssignment("unused", "deps");
            Assert.Equal("x = deps['module/path'].x", script);
        }

        [Fact]
        public void ImportFromAmdAssetAssignsVariableTheAmdModule()
        {
            var import = Import.FromAmdAsset("jquery", "$");
            var script = import.CreateAssignment("unused", "deps");
            Assert.Equal("$ = deps['jquery']", script);
        }
    }
}