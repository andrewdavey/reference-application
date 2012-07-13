using Cassette;
using Cassette.Scripts;
using Moq;
using Xunit;

namespace App.Infrastructure.Cassette
{
    public class AmdModuleTests
    {
        [Fact]
        public void WhenCreateAmdModuleFromBundleThenPathIsBundlePathWithoutTildePrefix()
        {
            var bundle = new ScriptBundle("~/test");
            var module = new AmdModule(bundle);
            Assert.Equal("test", module.Path);
        }

        [Fact]
        public void WhenCreateAmdModuleFromAssetThenPathIsTheAssetPathWithoutTildePrefixAndWithoutFileExtension()
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns("~/test/jquery.js");

            var module = new AmdModule(asset.Object, "$");

            Assert.Equal("test/jquery", module.Path);
        }

        [Fact]
        public void WhenCreateAmdModuleFromAssetThenAliasIsAssigned()
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns("~/test/jquery.js");

            var module = new AmdModule(asset.Object, "$");

            Assert.Equal("$", module.Alias);
        }
    }
}