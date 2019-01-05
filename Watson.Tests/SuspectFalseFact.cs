using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Events;
using NSubstitute;
using Watson.Domain;
using Watson.Domain.RegisterDocument;
using Watson.Domain.SuspectFalseFact;
using Watson.Infrastructure;
using Xunit;

namespace Watson.Tests
{
    public class SuspectFalseFact
    {
        private const string UNREACHABLE_WEB_PAGE = "https://wwww.unreachable/xx.html";

        private ICommandSender _commandSender;
        private IEventPublisher _eventPublisher;
        private IWebSiteChecker _webSiteChecker;

        public SuspectFalseFact()
        {
            var builder = new StructureMapContainerBuilder();
            var container = builder.Build();

            container.Inject(Substitute.For<IEventPublisher>());
            container.Inject(Substitute.For<IWebSiteChecker>());
            container.Inject<IEventStore>(container.GetInstance<InMemoryEventStore>());

            _commandSender = container.GetInstance<ICommandSender>();
            _eventPublisher = container.GetInstance<IEventPublisher>();
            _webSiteChecker = container.GetInstance<IWebSiteChecker>();
            _webSiteChecker.IsOnline(Arg.Any<string>()).Returns(Task.FromResult(true));
        }

        [Fact]
        public async Task publish_suspicious_fact_detected()
        {
            // Arrange
            var command = new SuspectFalseFactCommand
            {
                Text = "Our president has been elected by more that 60% of the population.",
                WebPageUrl = "https://wwww.fakenews/president.html",
                FirstSelectedHtmlNodeXPath = "//*[@id=\"content\"]/div/div/div[1]/div/div/div/div[3]/p[6]",
                LastSelectedHtmlNodeXPath = "//*[@id=\"content\"]/div/div/div[1]/div/div/div/div[3]/p[6]",
                SelectedTextStartOffset = 1,
                SelectedTextEndOffset = 62
            };

            // Act
            await _commandSender.Send(command);

            // Assert
            await _eventPublisher.Received(1).Publish(Arg.Is<SuspiciousFactDetected>(@event =>
                @event.Id != default(Guid) &&
                @event.FactContent == command.Text &&
                @event.WebPageUrl == command.WebPageUrl &&
                @event.Location.FirstSelectedHtmlNodeXPath == command.FirstSelectedHtmlNodeXPath &&
                @event.Location.LastSelectedHtmlNodeXPath == command.LastSelectedHtmlNodeXPath &&
                @event.Location.SelectedTextStartOffset == command.SelectedTextStartOffset &&
                @event.Location.SelectedTextEndOffset == command.SelectedTextEndOffset
            ));
        }

        [Fact]
        public async Task throw_exception_when_web_page_unreachable()
        {
            // Arrange
            _webSiteChecker.IsOnline(UNREACHABLE_WEB_PAGE).Returns(false);

            // Act
            await Assert.ThrowsAsync<UnreachableWebPage>(async () => {
                var command = new SuspectFalseFactCommand {
                    WebPageUrl = UNREACHABLE_WEB_PAGE
                };
                await _commandSender.Send(command);
            });
        }
    }
}