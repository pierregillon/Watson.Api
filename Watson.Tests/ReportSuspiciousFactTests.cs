using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Events;
using NSubstitute;
using Watson.Domain;
using Watson.Domain.ReportSuspiciousFact;
using Watson.Infrastructure;
using Xunit;

namespace Watson.Tests
{
    public class ReportSuspiciousFactTests
    {
        private const string UNREACHABLE_WEB_PAGE = "https://wwww.unreachable/xx.html";
        private const string REACHABLE_WEB_PAGE = "https://wwww.fakenews/president.html";
        private const string SOME_XMAP = "//*[@id=\"content\"]/div/div/div[1]/div/";
        private const string SOME_WORDING = "This president was fairly elected.";

        private ICommandSender _commandSender;
        private IEventPublisher _eventPublisher;
        private IWebSiteChecker _webSiteChecker;

        public ReportSuspiciousFactTests()
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
            var command = new ReportSuspiciousFactCommand {
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
                @event.Location.FirstNodeXPath == command.FirstSelectedHtmlNodeXPath &&
                @event.Location.LastNodeXPath == command.LastSelectedHtmlNodeXPath &&
                @event.Location.StartOffset == command.SelectedTextStartOffset &&
                @event.Location.EndOffset == command.SelectedTextEndOffset
            ));
        }

        [Fact]
        public async Task throw_exception_when_web_page_unreachable()
        {
            await Assert.ThrowsAsync<UnreachableWebPage>(async () => {
                // Arrange
                var command = new ReportSuspiciousFactCommand {
                    WebPageUrl = UNREACHABLE_WEB_PAGE
                };

                // Act
                await _commandSender.Send(command);
            });
        }

        [Theory]
        [InlineData("The president ")]
        [InlineData("I want")]
        [InlineData("bad example !")]
        [InlineData("No.")]
        [InlineData("inconsistency / magnificent")]
        public async Task throw_exception_when_not_enough_words(string wordingSample)
        {
            await Assert.ThrowsAsync<NotEnoughWords>(async () => {
                // Arrange
                var command = new ReportSuspiciousFactCommand  {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = wordingSample
                };

                // Act
                await _commandSender.Send(command);
            });
        }

        [Theory]
        [InlineData("First, you can start off by reading a basic example dialogue in Romaji, Japanese characters, and then English. Next, you'll find a chart of vocabulary words and common expressions that should be used in a restaurant setting. First, you can start off by reading a basic example dialogue in Romaji, Japanese characters, and then English. Next, you'll find a chart of vocabulary words and common expressions that should be used in a restaurant setting.")]
        public async Task throw_exception_when_to_many_words(string wordingSample)
        {
            await Assert.ThrowsAsync<ToManyWords>(async () => {
                // Arrange
                var command = new ReportSuspiciousFactCommand  {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = wordingSample
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
                var command = new ReportSuspiciousFactCommand {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = SOME_WORDING
                };

                // Act
                await _commandSender.Send(command);
            });
        }
    }
}