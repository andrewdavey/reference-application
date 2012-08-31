using Cassette;
using Cassette.TinyIoC;

namespace App.Infrastructure.Amd
{
    public class CassetteContainerConfiguration :IConfiguration<TinyIoCContainer>
    {
        public void Configure(TinyIoCContainer container)
        {
            container.Register<AmdModuleCollection>().AsSingleton();
        }
    }
}