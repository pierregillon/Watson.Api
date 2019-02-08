using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using Watson.Infrastructure;
using StructureMap;
using Watson.Server;
using Watson.Domain.ReportSuspiciousFact;
using Watson.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using System;
using CQRSlite.Queries;

namespace Watson.Infrastructure
{
    public class StructureMapContainerBuilder
    {
        public IContainer Build(AppSettings settings)
        {
            return new Container(x =>
            {
                x.For<AppSettings>().Use(settings).Singleton();
                x.For<ICommandSender>().Use<StructureMapCommandSender>().Singleton();
                x.For<IQueryProcessor>().Use<StructureMapQueryProcessor>().Singleton();
                x.For<IEventPublisher>().Use<StructureMapEventPublisher>().Singleton();
                x.For(typeof(IRepository)).Use(typeof(Repository));
                x.For<IWebSiteChecker>().Use<HttpWebRequestChecker>();
                x.For<InMemoryDatabase>().Singleton();

                x.For<ElasticSearchLogger>()
                    .Use<ElasticSearchLogger>()
                    .Ctor<string>("server").Is(settings.ElasticSearch.Server)
                    .Ctor<int>("port").Is(settings.ElasticSearch.Port)
                    .Ctor<string>("login").Is(settings.ElasticSearch.User)
                    .Ctor<string>("password").Is(settings.ElasticSearch.Password)
                    .Singleton();

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
                    scanner.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                });

                x.Scan(scanner => {
                    scanner.AssemblyContainingType(typeof(Bootstrapper));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));
                });
            });
        }
    }
}