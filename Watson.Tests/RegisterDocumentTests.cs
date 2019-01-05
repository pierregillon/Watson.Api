using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Events;
using Watson.Server;
using Watson.Infrastructure;
using NSubstitute;
using StructureMap;
using Xunit;
using Watson.Domain.RegisterDocument;
using Watson.Domain;

namespace watson.tests
{
    public class RegisterDocumentTests
    {
        private readonly string OFFLINE_WEB_SITE = "https://abcdefxx.com";

        private readonly ICommandSender commandSender;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebSiteChecker _webSiteChecker;

        public RegisterDocumentTests()
        {
            var builder = new StructureMapContainerBuilder();
            var container = builder.Build();

            container.Inject(Substitute.For<IEventPublisher>());
            container.Inject(Substitute.For<IWebSiteChecker>());
            container.Inject<IEventStore>(container.GetInstance<InMemoryEventStore>());

            commandSender = container.GetInstance<ICommandSender>();
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
            await commandSender.Send<RegisterDocumentCommand>(command);

            // Assert
            await _eventPublisher.Received(1).Publish(Arg.Any<DocumentRegistered>());
        }

        [Fact]
        public async Task throw_unreachable_web_site()
        {
            // Arrange
            _webSiteChecker.IsOnline(OFFLINE_WEB_SITE).Returns(Task.FromResult(false));

            // Act
            await Assert.ThrowsAsync<UnreachableWebPage>(async () =>
            {
                var command = new RegisterDocumentCommand(OFFLINE_WEB_SITE);
                await commandSender.Send<RegisterDocumentCommand>(command);
            });
        }
    }
}
