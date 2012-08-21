using System;
using System.IO;
using System.Text;
using Cassette;
using Cassette.Scripts;
using Moq;
using Xunit;

namespace App.Infrastructure.Amd
{
    public class AmdModuleFromBundleTests
    {
        [Fact]
        public void PathIsBundlePathWithoutTildeSlashPrefix()
        {
            var bundle = new ScriptBundle("~/test");
            var module = new AmdModuleFromBundle(bundle, path => null);
            Assert.Equal("test", module.Path);
        }

        [Fact]
        public void ReferenceCommentsDetermineModuleDependencies()
        {
            var bundle = new ScriptBundle("~/test")
            {
                Assets =
                {
                    StubAsset("~/test/a.js", "/// <reference path=\"~/jquery.js\" />")
                }
            };

            var jquery = Mock.Of<IAmdModule>();
            Func<string, IAmdModule> resolveReferencePathIntoAmdModule = path => jquery;
            var module = new AmdModuleFromBundle(bundle, resolveReferencePathIntoAmdModule);

            Assert.Equal(new[] {jquery}, module.Dependencies);
        }

        [Fact]
        public void DependenciesAreDistinct()
        {
            var bundle = new ScriptBundle("~/test")
            {
                Assets =
                {
                    StubAsset("~/test/a.js", "/// <reference path=\"~/jquery.js\" />"),
                    StubAsset("~/test/b.js", "/// <reference path=\"~/jquery.js\" />"),
                }
            };

            var jquery = Mock.Of<IAmdModule>();
            Func<string, IAmdModule> resolveReferencePathIntoAmdModule = path => jquery;
            var module = new AmdModuleFromBundle(bundle, resolveReferencePathIntoAmdModule);

            Assert.Equal(new[] { jquery }, module.Dependencies);
        }

        [Fact]
        public void ExportIsObjectContainingGlobalVars()
        {
            var bundle = new ScriptBundle("~/test")
            {
                Assets =
                {
                    StubAsset("~/test/a.js", "var a = 1;"),
                    StubAsset("~/test/b.js", "var b = 2;"),
                }
            };

            var module = new AmdModuleFromBundle(bundle, path => null);

            var export = (ObjectExport)module.Export;
            Assert.Equal(new[] {"a", "b"}, export.Aliases);
        }

        IAsset StubAsset(string path, string content)
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns(path);
            asset.Setup(a => a.OpenStream()).Returns(() => new MemoryStream(Encoding.UTF8.GetBytes(content)));
            return asset.Object;
        }
    }
}