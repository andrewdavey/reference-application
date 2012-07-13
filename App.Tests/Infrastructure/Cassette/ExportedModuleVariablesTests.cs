using Cassette;
using Moq;
using Xunit;

namespace App.Infrastructure.Cassette
{
    public class ExportedModuleVariablesTests
    {
        [Fact]
        public void SingleTopLevelVariableIsExported()
        {
            var asset = Mock.Of<IAsset>();
            asset.RecordExportedVariables("var x = 1;");
            Assert.Equal(new[] {"x"}, asset.GetExportedVariables());
        }

        [Fact]
        public void SingleTopLevelVarWithTwoVariablesExportsBoth()
        {
            var asset = Mock.Of<IAsset>();
            asset.RecordExportedVariables("var x = 1, y = 2;");
            Assert.Equal(new[] { "x", "y" }, asset.GetExportedVariables());
        }

        [Fact]
        public void TwoTopLevelVariablesExportsBoth()
        {
            var asset = Mock.Of<IAsset>();
            asset.RecordExportedVariables("var x = 1; var y = 2;");
            Assert.Equal(new[] { "x", "y" }, asset.GetExportedVariables());
        }
    }
}