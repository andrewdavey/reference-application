using Cassette;
using Moq;
using Xunit;

namespace App.Infrastructure.Amd
{
    public class VendorAmdModuleTests
    {
        [Fact]
        public void PathIsAssetPathWithoutTildeSlashPrefixOrFileExtension()
        {
            var asset = StubAsset("~/jquery.js");
            var module = new VendorAmdModule(asset, "$");

            Assert.Equal("jquery", module.Path);
        }

        [Fact]
        public void Exports()
        {
            var asset = StubAsset("~/jquery.js");
            var module = new VendorAmdModule(asset, "$");

            var export = (SingleValueExport)module.Export;
            Assert.Equal("$", export.Identifier);
        }

        IAsset StubAsset(string path)
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns(path);
            return asset.Object;
        }
    }
}