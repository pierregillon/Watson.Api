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
        private const string SOME_XPATH = "//*[@id=\"content\"]/div/div/div[1]/div/text()";
        private const string SOME_WORDING = "This president was fairly elected.";

        private ICommandSender _commandSender;
        private IEventPublisher _eventPublisher;
        private IWebSiteChecker _webSiteChecker;

        public ReportSuspiciousFactTests()
        {
            var builder = new StructureMapContainerBuilder();
            var container = builder.Build(new Server.AppSettings());

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
                StartNodeXPath = "//*[@id=\"content\"]/div/div/div[1]/div/div/div/div[3]/p[6]/text()",
                EndNodeXPath = "//*[@id=\"content\"]/div/div/div[1]/div/div/div/div[3]/p[6]/text()",
                StartOffset = 10,
                EndOffset = 76
            };

            // Act
            await _commandSender.Send(command);

            // Assert
            await _eventPublisher.Received(1).Publish(Arg.Is<SuspiciousFactDetected>(@event =>
                @event.Id != default(Guid) &&
                @event.Wording == command.Wording &&
                @event.WebPageUrl == command.WebPageUrl &&
                @event.Location.StartNodeXPath.ToString() == command.StartNodeXPath &&
                @event.Location.EndNodeXPath.ToString() == command.EndNodeXPath &&
                @event.Location.StartOffset == command.StartOffset &&
                @event.Location.EndOffset == command.EndOffset
            ));
        }

        [Fact]
        public async Task clear_wording_from_duplicated_spaces()
        {
            // Arrange
            var command = new ReportSuspiciousFactCommand {
                Wording = "Our president has been elected                   by more that 60% of the population.",
                WebPageUrl = REACHABLE_WEB_PAGE,
                StartNodeXPath = SOME_XPATH,
                EndNodeXPath = SOME_XPATH,
                StartOffset = 10,
                EndOffset = 76
            };

            // Act
            await _commandSender.Send(command);

            // Assert
            await _eventPublisher.Received(1).Publish(Arg.Is<SuspiciousFactDetected>(@event =>
                @event.Wording == "Our president has been elected by more that 60% of the population."
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
                    Wording = wordingSample,
                    StartNodeXPath = SOME_XPATH,
                    EndNodeXPath = SOME_XPATH
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
                    Wording = wordingSample,
                    StartNodeXPath = SOME_XPATH,
                    EndNodeXPath = SOME_XPATH
                };

                // Act
                await _commandSender.Send(command);
            });
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("qsf", "abjlqsdfj")]
        [InlineData("/html", "/html")]
        public async Task throw_exception_when_invalid_html_xpath_location(string beginXPath, string endXPath)
        {
            await Assert.ThrowsAsync<InvalidXPathFormat>(async () => {
                // Arrange
                var command = new ReportSuspiciousFactCommand {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = SOME_WORDING,
                    StartNodeXPath = beginXPath,
                    EndNodeXPath = endXPath
                };

                // Act
                await _commandSender.Send(command);
            });
        }

        [Theory]
        [InlineData("/html/body/p[1]/text()")]
        [InlineData("//*[@id=\"myID\"]/ul/li[1]/div/div[1]/div/div/h13/a/text()")]
        public async Task accept_different_xpath_format(string xpath) {

            // Arrange
            var command = new ReportSuspiciousFactCommand {
                Wording = "Our president has been elected by more that 60% of the population.",
                WebPageUrl = "https://wwww.fakenews/president.html",
                StartNodeXPath = xpath,
                EndNodeXPath = xpath,
                StartOffset = 10,
                EndOffset = 76
            };

            // Act
            await _commandSender.Send(command);
        }

        [Theory]
        [InlineData("p", "div")]
        [InlineData("p[1]", "p[2]")]
        [InlineData("p[1]", "p[3]")]
        [InlineData("div[1]", "div[2]")]
        [InlineData("li[1]", "li[2]")]
        public async Task throw_exception_when_fact_spread_over_multiple_paragraphs(string beginElement, string endElement)
        {
            await Assert.ThrowsAsync<FactSpreadOverMultipleParagraphs>(async () => {
                // Arrange
                var command = new ReportSuspiciousFactCommand {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = SOME_WORDING,
                    StartNodeXPath = $"/html/body/{beginElement}/text()",
                    EndNodeXPath = $"/html/body/{endElement}/text()",
                    StartOffset = 0,
                    EndOffset = 5
                };

                // Act
                await _commandSender.Send(command);
            });
        }

        [Theory]
        [InlineData("/html/body/p[1]/text()", "/html/body/p[1]/span/span/text()")]
        [InlineData("/html/body/p/text()[1]", "/html/body/p/text()[4]")]
        [InlineData("/html/body/p/text()[1]", "/html/body/p/a/strong/text()")]
        [InlineData("/html/body/p/a/text()", "/html/body/p/strong/text()")]
        [InlineData("/html/body/p/a/text()", "/html/body/p/span/text()")]
        [InlineData("/html/body/p/a/text()", "/html/body/p/em/text()")]
        public async Task do_not_throw_exception_when_fact_in_same_paragraph(string beginXPath, string endXPath)
        {
            try
            {
                // Arrange
                var command = new ReportSuspiciousFactCommand {
                    WebPageUrl = REACHABLE_WEB_PAGE,
                    Wording = SOME_WORDING,
                    StartNodeXPath = beginXPath,
                    EndNodeXPath = endXPath,
                    StartOffset = 0,
                    EndOffset = 5
                };

                // Act
                await _commandSender.Send(command);
            }
            catch (FactSpreadOverMultipleParagraphs) {
                throw;
            }
            catch{}
        }
    }
}