using Xunit;

namespace App.Infrastructure.Cassette
{
    public class DebugModuleBuilderTests
    {
        [Fact]
        public void BuildModuleThatHasNoAssetsOrDependencies()
        {
            var builder = new DebugModuleBuilder("test");
            var script = builder.Build();

            Assert.Equal(
@"debugModules['test']=[];
define(
    'test',
    [],
    function(){
        var module = {}, deps = {};
        debugModules['test'].forEach(function(init){
            init(module, deps);
        });
        return module;
    }
);", script);
        }

        [Fact]
        public void BuildModuleWithAssetUrls()
        {
            var builder = new DebugModuleBuilder("test");
            builder.AddAssetUrl("/cassette.axd/test1");
            builder.AddAssetUrl("/cassette.axd/test2");
            var script = builder.Build();

            Assert.Equal(
@"debugModules['test']=[];
define(
    'test',
    ['/cassette.axd/test1','/cassette.axd/test2'],
    function(){
        var module = {}, deps = {};
        debugModules['test'].forEach(function(init){
            init(module, deps);
        });
        return module;
    }
);", script);
        }

        [Fact]
        public void BuildModuleWithDependency()
        {
            var builder = new DebugModuleBuilder("test");
            builder.AddDependencies(new[] {"jquery"});
            var script = builder.Build();

            Assert.Equal(
@"debugModules['test']=[];
define(
    'test',
    ['jquery'],
    function(d0){
        var module = {}, deps = {'jquery':d0};
        debugModules['test'].forEach(function(init){
            init(module, deps);
        });
        return module;
    }
);", script);
        }

        [Fact]
        public void BuildModuleWithDependencies()
        {
            var builder = new DebugModuleBuilder("test");
            builder.AddDependencies(new[] { "jquery", "knockout" });
            var script = builder.Build();

            Assert.Equal(
@"debugModules['test']=[];
define(
    'test',
    ['jquery','knockout'],
    function(d0,d1){
        var module = {}, deps = {'jquery':d0,'knockout':d1};
        debugModules['test'].forEach(function(init){
            init(module, deps);
        });
        return module;
    }
);", script);
        }

        [Fact]
        public void BuildModuleWithAssetsAndDependencies()
        {
            var builder = new DebugModuleBuilder("test");
            builder.AddAssetUrl("/cassette.axd/test1");
            builder.AddAssetUrl("/cassette.axd/test2");
            builder.AddDependencies(new[] { "jquery", "knockout" });
            var script = builder.Build();

            Assert.Equal(
@"debugModules['test']=[];
define(
    'test',
    ['jquery','knockout','/cassette.axd/test1','/cassette.axd/test2'],
    function(d0,d1){
        var module = {}, deps = {'jquery':d0,'knockout':d1};
        debugModules['test'].forEach(function(init){
            init(module, deps);
        });
        return module;
    }
);", script);
        }

        [Fact]
        public void DuplicateDependenciesAreIgnored()
        {
            var builder = new DebugModuleBuilder("test");
            builder.AddDependencies(new[] { "jquery", "jquery", "knockout" });
            builder.AddDependencies(new[] { "jquery" });
            var script = builder.Build();

            Assert.Equal(
@"debugModules['test']=[];
define(
    'test',
    ['jquery','knockout'],
    function(d0,d1){
        var module = {}, deps = {'jquery':d0,'knockout':d1};
        debugModules['test'].forEach(function(init){
            init(module, deps);
        });
        return module;
    }
);", script);

        }
    }
}