using System.IO;
using System.Text;
using Cassette;
using Cassette.IO;
using Cassette.Scripts;
using Moq;
using Xunit;

namespace App.Infrastructure.Amd
{
    public class DebugAmdModuleFromBundleTests
    {
        ScriptBundle bundle;

        [Fact]
        public void EachAssetIsWrappedInDebugModuleDefinition()
        {
            bundle = new ScriptBundle("~/test") { Pipeline = new SimplePipeline() };
            AddAsset("~/a.js", "var a = 1;");
            AddAsset("~/b.js", "var b = 2;");
            
            var module = new DebugAmdModuleFromBundle(bundle, path => null);
            bundle.Pipeline.Process(bundle);

            AssertAssetContent(bundle.Assets[0], "define([],function(){return function(){var a = 1;this.a=a;}});");
            AssertAssetContent(bundle.Assets[1], "define([],function(){return function(){var a=this.a;var b = 2;this.b=b;}});");
        }

        [Fact]
        public void DefinitionShimHasDependencyOnAssets()
        {
            bundle = new ScriptBundle("~/test") { Pipeline = new SimplePipeline() };
            AddAsset("~/a.js", "var a = 1;");
            AddAsset("~/b.js", "var b = 2;");

            var module = new DebugAmdModuleFromBundle(bundle, path => null);

            Assert.Equal("define(\"test\",[\"a.js\",\"b.js\"],function(){\n" +
                         "var exports={};\n" +
                         "var assets=Array.prototype.slice.call(arguments,0);\n" +
                         "var dependencies=Array.prototype.slice.call(arguments,0,0);\n" +
                         "assets.forEach(function(a) { a.apply(exports, dependencies); });\n" +
                         "return exports;\n" +
                         "});", module.DefinitionShim);
        }

        [Fact]
        public void DefinitionShimHasDependencyOnModuleDependencies()
        {
            bundle = new ScriptBundle("~/test") { Pipeline = new SimplePipeline() };
            AddAsset("~/a.js", "/// <reference path=\"~/jquery.js\"/>");

            var jquery = new Mock<IAmdModule>();
            jquery.SetupGet(j => j.Path).Returns("jquery");
            var module = new DebugAmdModuleFromBundle(bundle, path => jquery.Object);

            Assert.Equal("define(\"test\",[\"jquery\",\"a.js\"],function(){\n" +
                         "var exports={};\n" +
                         "var assets=Array.prototype.slice.call(arguments,1);\n" +
                         "var dependencies=Array.prototype.slice.call(arguments,0,1);\n" +
                         "assets.forEach(function(a) { a.apply(exports, dependencies); });\n" +
                         "return exports;\n" +
                         "});", module.DefinitionShim);
        }

        [Fact]
        public void ProcessAddsShimAssetToBundle()
        {
            bundle = new ScriptBundle("~/test") { Pipeline = new SimplePipeline() };
            AddAsset("~/a.js", "");

            var module = new DebugAmdModuleFromBundle(bundle, path => null);

            module.Process(bundle);
            Assert.Equal("~/test/debug-shim.js", bundle.Assets[1].Path);

            using (var reader = new StreamReader(bundle.Assets[1].OpenStream()))
            {
                Assert.Equal(module.DefinitionShim, reader.ReadToEnd());
            }
        }

        void AssertAssetContent(IAsset asset, string content)
        {
            using (var reader = new StreamReader(asset.OpenStream()))
            {
                var outputA = reader.ReadToEnd();
                Assert.Equal(content, outputA);
            }
        }

        void AddAsset(string path, string content)
        {
            var file = new Mock<IFile>();
            file.SetupGet(f => f.FullPath)
                .Returns(path);
            file.Setup(f => f.Open(It.IsAny<FileMode>(), It.IsAny<FileAccess>(), It.IsAny<FileShare>()))
                .Returns(() => new MemoryStream(Encoding.UTF8.GetBytes(content)));
            var asset = new FileAsset(file.Object, bundle);
            bundle.Assets.Add(asset);
        }
    }
}