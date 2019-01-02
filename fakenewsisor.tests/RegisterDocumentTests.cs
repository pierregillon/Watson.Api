using System;
using System.Threading.Tasks;
using fakenewsisor.server;
using fakenewsisor.server.Infrastructure;
using NSubstitute;
using StructureMap;
using try4real.cqrs;
using try4real.cqrs.structuremap;
using try4real.ddd;
using try4real.ddd.structuremap;
using Xunit;

namespace fakenewsisor.tests
{
    public class RegisterDocumentTests
    {
        private readonly string OFFLINE_WEB_SITE = "https://abcdefxx.com";

        private readonly ICommandDispatcher _dispatcher;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebSiteChecker _webSiteChecker;

        public RegisterDocumentTests()
        {
            var builder = new StructureMapContainerBuilder();
            var container = builder.Build();

            container.Inject(Substitute.For<IEventPublisher>());
            container.Inject(Substitute.For<IWebSiteChecker>());
            container.Inject<IEventStore>(container.GetInstance<InMemoryEventStore>());

            _dispatcher = container.GetInstance<ICommandDispatcher>();
            _eventPublisher = container.GetInstance<IEventPublisher>();
            _webSiteChecker = container.GetInstance<IWebSiteChecker>();
            _webSiteChecker.IsOnline(Arg.Any<string>()).Returns(Task.FromResult(true));
        }

        [Fact]
        public async Task publish_document_registered()
        {
            // Arrange
            var command = new RegisterDocumentCommand("https://google.com");

            // Act
            await _dispatcher.Dispatch<RegisterDocumentCommand, Guid>(command);

            // Assert
            _eventPublisher.Received(1).Publish(Arg.Any<DocumentRegistered>());
        }

        [Fact]
        public async Task throw_unreachable_web_site()
        {
            // Arrange
            _webSiteChecker.IsOnline(OFFLINE_WEB_SITE).Returns(Task.FromResult(false));

            // Act
            await Assert.ThrowsAsync<UnreachableWebDocument>(async () =>
            {
                var command = new RegisterDocumentCommand(OFFLINE_WEB_SITE);
                await _dispatcher.Dispatch<RegisterDocumentCommand, Guid>(command);
            });
        }
    }
}
