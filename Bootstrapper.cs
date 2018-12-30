using Nancy.Bootstrappers.StructureMap;
using StructureMap;

namespace fakenewsisor.server
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(IContainer container)
        {
            container.Configure(x =>
            {
                x.For<ICommandDispatcher>().Use<StructureMapCommandDispatcher>();
                x.For<IEventEmitter>().Use<StructureMapEventEmitter>();
            });
        }
    }
}