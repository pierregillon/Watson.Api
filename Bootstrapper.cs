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
                x.For<IEventStore>().Use<InMemoryEventStore>().Singleton();
                x.For(typeof(IRepository<>)).Use(typeof(Repository<>));

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(ICommandHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                });

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(IEventListener<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IEventListener<>));
                });

                // Finders are in memory. They need to be a singleton
                // to process events.
                x.For<FalseInformationFinder>().Singleton();
                x.For<WebPageFinder>().Singleton();
            });
        }
    }
}