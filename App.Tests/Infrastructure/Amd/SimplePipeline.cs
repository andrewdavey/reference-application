using Cassette.BundleProcessing;
using Cassette.Scripts;
using Cassette.TinyIoC;

namespace App.Infrastructure.Amd
{
    class SimplePipeline : BundlePipeline<ScriptBundle>
    {
        public SimplePipeline() : base(new TinyIoCContainer())
        {
        }
    }
}