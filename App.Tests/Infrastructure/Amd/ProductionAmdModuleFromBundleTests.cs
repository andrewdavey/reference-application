using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cassette;
using Cassette.Scripts;
using Moq;
using Xunit;

namespace App.Infrastructure.Amd
{
    public class ProductionAmdModuleFromBundleTests
    {
        readonly ScriptBundle bundle;
        readonly List<IAmdModule> modules = new List<IAmdModule>();

        public ProductionAmdModuleFromBundleTests()
        {
            bundle = new ScriptBundle("~/test")
            {
                Pipeline = new SimplePipeline()
            };
        }

        [Fact]
        public void DependencyIdentiferGeneratesFunctionParameter()
        {
            GivenAsset("~/test/a.js", "/// <reference path=\"~/jquery.js\" />");
            GivenModule("jquery", new SingleValueExport("$"));

            var module = CreateModule();
            var output = module.WrapScriptInDefineCall("source;");

            Assert.Equal("define(\"test\",[\"jquery\"],function($){source;\nreturn {};\n});", output);
        }

        [Fact]
        public void DependencyAliasesGenerateVars()
        {
            GivenAsset("~/test/a.js", "/// <reference path=\"~/other/b.js\" />");
            GivenModule("other", new ObjectExport("__other", new[] {"b", "c"}));
            
            var module = CreateModule();
            var output = module.WrapScriptInDefineCall("source;");

            Assert.Equal("define(\"test\",[\"other\"],function(__other){" +
                         "var b=__other.b;" +
                         "var c=__other.c;" +
                         "source;\n" +
                         "return {};\n" +
                         "});", output);
        }

        [Fact]
        public void AssetGlobalVariablesReturnedAsModuleResult()
        {
            GivenAsset("~/test/a.js", "var x = 1, y = 2; var z = 3;");
            var module = CreateModule();
            var output = module.WrapScriptInDefineCall("source;");

            Assert.Equal("define(\"test\",[],function(){" +
                         "source;\n" +
                         "return {x:x,y:y,z:z};\n" +
                         "});", output);
        }

        void GivenAsset(string path, string content)
        {
            bundle.Assets.Add(StubAsset(path, content));
        }

        void GivenModule(IAmdModule module)
        {
            modules.Add(module);
        }

        void GivenModule(string path, IExport export)
        {
            var module = new Mock<IAmdModule>();
            module.SetupGet(m => m.Path).Returns(path);
            module.SetupGet(m => m.Export).Returns(export);
            GivenModule(module.Object);
        }

        ProductionAmdModuleFromBundle CreateModule()
        {
            return new ProductionAmdModuleFromBundle(bundle, GetReferencedModule, Mock.Of<IUrlGenerator>());
        }

        IAmdModule GetReferencedModule(string path)
        {
            path = path.TrimStart('~', '/');
            return modules.First(m => path.StartsWith(m.Path));
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