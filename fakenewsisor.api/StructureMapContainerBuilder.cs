using fakenewsisor.server.Infrastructure;
using StructureMap;
using try4real.cqrs;
using try4real.cqrs.structuremap;
using try4real.ddd;
using try4real.ddd.structuremap;

namespace fakenewsisor.server
{
    public class StructureMapContainerBuilder
    {
        public IContainer Build()
        {
            return new Container(x =>
            {
                x.For<ICommandDispatcher>().Use<StructureMapCommandDispatcher>();
                x.For<IEventPublisher>().Use<StructureMapEventPublisher>();
                x.For<IEventStore>().Use<InMemoryEventStore>().Singleton();
                x.For(typeof(IRepository<>)).Use(typeof(Repository<>));
                x.For<IWebSiteChecker>().Use<HttpWebRequestChecker>();
                x.For<InMemoryDatabase>().Singleton();

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(Bootstrapper));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                });

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(Bootstrapper));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                });

                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof(Bootstrapper));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IEventListener<>));
                });
            });
        }
    }
}