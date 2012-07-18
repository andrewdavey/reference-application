using System;
using System.Collections.Generic;
using System.Linq;
using Cassette;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    //public class ShimProvider : IStartUpTask
    //{
    //    readonly BundleCollection bundles;
    //    public static string Shims { get; private set; }

    //    public ShimProvider(BundleCollection bundles)
    //    {
    //        this.bundles = bundles;
    //    }

    //    public void Start()
    //    {
    //        bundles.Changed += bundles_Changed;
    //    }

    //    void bundles_Changed(object sender, BundleCollectionChangedEventArgs e)
    //    {
    //        var shims = e
    //            .Bundles
    //            .OfType<ScriptBundle>()
    //            .Where(b => b.GetMetaDataOrDefault("AmdModulePerAsset", false))
    //            .SelectMany(b => AssetCollector.Collect(b, a => a.GetMetaData<Shim>("AmdShim")));


    //    }
    //}

    //class ShimCollector : IBundleVisitor
    //{
    //    public static IEnumerable<IAsset> Collect(Bundle bundle, Func<IAsset,bool> predicate)
    //    {
    //        var collector = new AssetCollector(predicate);
    //        bundle.Accept(collector);
    //        return collector.collected;
    //    } 

    //    readonly Func<IAsset, bool> predicate;
    //    readonly List<IAsset> collected = new List<IAsset>(); 

    //    public AssetCollector(Func<IAsset,bool> predicate)
    //    {
    //        this.predicate = predicate;
    //    }

    //    public void Visit(Bundle bundle)
    //    {
    //    }

    //    public void Visit(IAsset asset)
    //    {
    //        if (predicate(asset))
    //        {
    //            collected.Add(asset);
    //        }
    //    }
    //}

    //public class Shim
    //{
    //    readonly string moduleVariable;
    //    readonly IEnumerable<string> dependencies;

    //    public Shim(string moduleVariable, IEnumerable<string> dependencies)
    //    {
    //        this.moduleVariable = moduleVariable;
    //        this.dependencies = dependencies;
    //    }

    //    public string ModuleVariable
    //    {
    //        get { return moduleVariable; }
    //    }

    //    public IEnumerable<string> Dependencies
    //    {
    //        get { return dependencies; }
    //    }
    //}
}