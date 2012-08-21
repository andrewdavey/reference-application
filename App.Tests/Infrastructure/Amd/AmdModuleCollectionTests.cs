using System.IO;
using System.Linq;
using System.Text;
using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;
using Moq;
using Xunit;

namespace App.Infrastructure.Amd
{
    public class AmdModuleCollectionTests
    {
        readonly Mock<IUrlGenerator> urlGenerator;
        readonly CassetteSettings settings;
        readonly AmdModuleCollection collection;

        public AmdModuleCollectionTests()
        {
            urlGenerator = new Mock<IUrlGenerator>();
            settings = new CassetteSettings(Enumerable.Empty<IConfiguration<CassetteSettings>>());
            collection = new AmdModuleCollection(urlGenerator.Object, settings);
        }

        [Fact]
        public void GivenProductionModeThenRequirePathsContainsBundleUrl()
        {
            var bundle = new ScriptBundle("~/test") {Pipeline = new SimplePipeline()};
            bundle.Assets.Add(StubAsset("~/test/a.js"));

            collection.AddModuleFromBundle(bundle);

            bundle.Pipeline.Process(bundle);

            urlGenerator.Setup(g => g.CreateBundleUrl(bundle)).Returns("/URL");
            Assert.Equal("/URL?noext=1", collection.Require.Paths["test"]);
        }

        [Fact]
        public void GivenDebugModeThenRequirePathsContainsShimAssetUrl()
        {
            settings.IsDebuggingEnabled = true;

            var pipeline = new SimplePipeline { new SortAssetsByDependency() };
            var bundle = new ScriptBundle("~/test") { Pipeline = pipeline };
            bundle.Assets.Add(StubAsset("~/test/a.js"));
            collection.AddModuleFromBundle(bundle);
            bundle.Pipeline.Process(bundle);

            var shimAsset = bundle.Assets.Last();
            urlGenerator.Setup(g => g.CreateAssetUrl(shimAsset)).Returns("/URL.js");

            Assert.Equal("/URL.js?noext=1", collection.Require.Paths["test"]);
        }

        IAsset StubAsset(string path)
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns(path);
            asset.Setup(a => a.OpenStream()).Returns(() => new MemoryStream(Encoding.UTF8.GetBytes("")));
            return asset.Object;
        }
    }
}