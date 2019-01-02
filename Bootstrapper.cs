using fakenewsisor.server.Infrastructure;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;
using try4real.cqrs;
using try4real.cqrs.structuremap;
using try4real.ddd;
using try4real.ddd.structuremap;

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
                x.For<IWebSiteChecker>().Use<HttpWebRequestChecker>();
                x.For<InMemoryDatabase>().Singleton();

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(ICommandHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                });

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(ICommandHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                });

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(IEventListener<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IEventListener<>));
                });
            });
        }
    }
}