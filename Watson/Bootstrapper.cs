using Watson.Infrastructure;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;

namespace Watson.Server
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            var containerBuilder = new StructureMapContainerBuilder();
            return containerBuilder.Build();
        }
    }
}