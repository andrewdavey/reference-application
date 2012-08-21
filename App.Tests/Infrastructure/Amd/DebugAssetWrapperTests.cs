using Xunit;

namespace App.Infrastructure.Amd
{
    public class DebugAssetWrapperTests
    {
        [Fact]
        public void ExportedVariableAttachedToThis()
        {
            var output = DebugAssetWrapper.Wrap("source", new[] { "x" }, new string[0], new IExport[0]);

            Assert.Equal("define([],function(){" +
                         "return function(){" +
                         "source" +
                         "this.x=x;" +
                         "}" +
                         "});", output);
        }

        [Fact]
        public void ImportedSingleValueExportGeneratesParameter()
        {
            var output = DebugAssetWrapper.Wrap("source", new[] { "x" }, new string[0], new IExport[] { new SingleValueExport("$") });

            Assert.Equal("define([],function(){" +
                         "return function($){" +
                         "source" +
                         "this.x=x;" +
                         "}" +
                         "});", output);
        }

        [Fact]
        public void AliasesVariablesFromPreviousAssetsInSameModule()
        {
            var output = DebugAssetWrapper.Wrap("source", new string[0], new[] { "x", "y" }, new IExport[0]);

            Assert.Equal("define([],function(){" +
                         "return function(){" +
                         "var x=this.x;" +
                         "var y=this.y;" +
                         "source" +
                         "}" +
                         "});", output);
        }

        [Fact]
        public void AliasesVariablesFromOtherModules()
        {
            var output = DebugAssetWrapper.Wrap("source", new string[0], new string[0], new IExport[] { new ObjectExport("other", new[] { "x", "y" }), });

            Assert.Equal("define([],function(){" +
                         "return function(other){" +
                         "var x=other.x;" +
                         "var y=other.y;" +
                         "source" +
                         "}" +
                         "});", output);
        }
    }
}