using fakenewsisor.server.DDD_CQRS;
using fakenewsisor.server.DDD_CQRS.StructureMap;
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
                x.For<IEventPublisher>().Use<StructureMapEventPublisher>();

                // The finder is in memory. It need to be a singleton
                // to process events.
                x.For<FalseInformationFinder>().Singleton();
            });
        }
    }
}