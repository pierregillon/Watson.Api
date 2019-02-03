using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using Watson.Infrastructure;
using StructureMap;
using Watson.Server;
using Watson.Domain.ReportSuspiciousFact;
using Watson.Infrastructure.Logging;

namespace Watson.Infrastructure
{
    public class StructureMapContainerBuilder
    {
        public IContainer Build()
        {
            return new Container(x =>
            {
                x.For<ICommandSender>().Use<StructureMapCommandSender>().Singleton();
                x.For<IEventPublisher>().Use<StructureMapEventPublisher>().Singleton();
                x.For(typeof(IRepository)).Use(typeof(Repository));
                x.For<IWebSiteChecker>().Use<HttpWebRequestChecker>();
                x.For<InMemoryDatabase>().Singleton();
                x.For<ElasticSearchLogger>().Use<ElasticSearchLogger>(context => new ElasticSearchLogger());
                x.For<ILogger>()
                    .Use<LoggerBroadcaster>(context => new LoggerBroadcaster(context.GetInstance<ConsoleLogger>(), context.GetInstance<ElasticSearchLogger>()))
                    .Singleton();
                x.For<ITypeLocator>().Use<ReflectionTypeLocator>();
                x.For<EventStoreOrg>().Use<EventStoreOrg>().Singleton();
                x.For<IEventStore>().Use(c => c.GetInstance<EventStoreOrg>());

                x.Scan(scanner => {
                    scanner.AssemblyContainingType(typeof(Bootstrapper));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                });

                x.Scan(scanner => {
                    scanner.AssemblyContainingType(typeof(Bootstrapper));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));
                });
            });
        }
    }
}