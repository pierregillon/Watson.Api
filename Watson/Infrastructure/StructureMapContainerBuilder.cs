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
using Watson.Authentication;
using Watson.Api;
using Watson.Api.Jwt;

namespace Watson.Infrastructure
{
    public class StructureMapContainerBuilder
    {
        public IContainer Build(AppSettings settings)
        {
            return new Container(x =>
            {
                // Api
                x.For<ITokenEncoder>().Use<JoseJwtTokenEncoder>();
                x.For<ITokenValidator>().Use<JwtTokenValidator>();
                x.For<AppSettings>().Use(settings).Singleton();

                // Application
                x.For<ICommandSender>().Use<StructureMapCommandSender>().Singleton();
                x.For<IQueryProcessor>().Use<StructureMapQueryProcessor>().Singleton();
                x.For<IEventPublisher>().Use<StructureMapEventPublisher>().Singleton();
                x.For<IRepository>().Use<Repository>();

                // Infrastructure
                x.For<InMemoryDatabase>().Singleton();
                x.For<ElasticSearchLogger>()
                    .Use<ElasticSearchLogger>()
                    .Ctor<string>("server").Is(settings.ElasticSearch.Server)
                    .Ctor<int>("port").Is(settings.ElasticSearch.Port)
                    .Ctor<string>("login").Is(settings.ElasticSearch.User)
                    .Ctor<string>("password").Is(settings.ElasticSearch.Password)
                    .Singleton();

                x.For<ILogger>()
                    .Use<LoggerBroadcaster>(context => new LoggerBroadcaster(
                        context.GetInstance<ConsoleLogger>(), 
                        context.GetInstance<ElasticSearchLogger>())
                    )
                    .Singleton();
                    
                x.For<ITypeLocator>().Use<ReflectionTypeLocator>();
                x.For<EventStoreOrg>().Use<EventStoreOrg>().Singleton();
                x.For<IEventStore>().Use(context => context.GetInstance<EventStoreOrg>());
                x.For<IWebSiteChecker>().Use<HttpWebRequestChecker>();

                // Scans

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