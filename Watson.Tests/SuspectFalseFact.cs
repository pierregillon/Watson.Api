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
        private const string REACHABLE_WEB_PAGE = "https://wwww.fakenews/president.html";
        private const string SOME_XMAP = "//*[@id=\"content\"]/div/div/div[1]/div/";
        private const string SOME_WORDING = "test";

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
            _webSiteChecker.IsOnline(UNREACHABLE_WEB_PAGE).Returns(false);
        }

        [Fact]
        public async Task publish_suspicious_fact_detected()
        {
            // Arrange
            var command = new SuspectFalseFactCommand {
                Wording = "Our president has been elected by more that 60% of the population.",
                WebPageUrl = "https://wwww.fakenews/president.html",
                FirstSelectedHtmlNodeXPath = "//*[@id=\"content\"]/div/div/div[1]/div/div/div/div[3]/p[6]",
                LastSelectedHtmlNodeXPath = "//*[@id=\"content\"]/div/div/div[1]/div/div/div/div[3]/p[6]",
                SelectedTextStartOffset = 10,
                SelectedTextEndOffset = 76
            };

            // Act
            await _commandSender.Send(command);

            // Assert
            await _eventPublisher.Received(1).Publish(Arg.Is<SuspiciousFactDetected>(@event =>
                @event.Id != default(Guid) &&
                @event.Wording == command.Wording &&
                @event.WebPageUrl == command.WebPageUrl &&
                @event.Location.FirstSelectedHtmlNodeXPath == command.FirstSelectedHtmlNodeXPath &&
                @event.Location.LastSelectedHtmlNodeXPath == command.LastSelectedHtmlNodeXPath &&
                @event.Location.SelectedTextStartOffset == command.SelectedTextStartOffset &&
                @event.Location.SelectedTextEndOffset == command.SelectedTextEndOffset
            ));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task throw_exception_when_the_wording_invalid(string wording)
        {
            // Arrange
            _webSiteChecker.IsOnline(UNREACHABLE_WEB_PAGE).Returns(false);

            // Act
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var command = new SuspectFalseFactCommand {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = wording
                };
                await _commandSender.Send(command);
            });
        }
        [Fact]
        public async Task throw_exception_when_web_page_unreachable()
        {
            await Assert.ThrowsAsync<UnreachableWebPage>(async () => {
                // Arrange
                var command = new SuspectFalseFactCommand {
                    WebPageUrl = UNREACHABLE_WEB_PAGE
                };

                // Act
                await _commandSender.Send(command);
            });
        }

        [Fact]
        public async Task throw_exception_when_invalid_html_xmap_location()
        {
            await Assert.ThrowsAsync<InvalidHtmlLocation>(async () => {
                // Arrange
                var command = new SuspectFalseFactCommand {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = SOME_WORDING,
                    FirstSelectedHtmlNodeXPath = "",
                    LastSelectedHtmlNodeXPath = "",
                };

                // Act
                await _commandSender.Send(command);
            });
        }
        
        [Fact]
        public async Task throw_exception_when_inconsistent_wording_and_offsets()
        {
            await Assert.ThrowsAsync<FactWordingAndOffsetInconsistent>(async () => {
                // Arrange
                var command = new SuspectFalseFactCommand {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    FirstSelectedHtmlNodeXPath = SOME_XMAP,
                    LastSelectedHtmlNodeXPath = SOME_XMAP,
                    Wording = "It is in the bag.",
                    SelectedTextStartOffset = 0,
                    SelectedTextEndOffset = "It is in the bag.".Length - 5
                };

                // Act
                await _commandSender.Send(command);
            });
        }
    }
}