using System;
using CQRSlite.Domain;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Domain
{
    public class Fact : AggregateRoot
    {
        private const int MAXIMUM_WORD_COUNT = 50;
        private const int MINIMUM_WORD_COUNT = 3;

        public Fact(){}
        public Fact(string wording, string webPageUrl, HtmlLocation location)
        {
            if (string.IsNullOrEmpty(wording)) {
                throw new ArgumentException("wording", nameof(wording));
            }

            var wordCount = wording.WordCount();
            if (wordCount < MINIMUM_WORD_COUNT) {
                throw new NotEnoughWords(MINIMUM_WORD_COUNT);
            }
            else if( wordCount > MAXIMUM_WORD_COUNT) {
                throw new ToManyWords(MAXIMUM_WORD_COUNT);
            }

            if(location.StartNodeXPath.IsInSameParagraph(location.EndNodeXPath) == false) {
                throw new FactSpreadOverMultipleParagraphs();
            }
            
            ApplyChange(new SuspiciousFactDetected(Guid.NewGuid(), wording, webPageUrl, location));
        }

        private void Apply(SuspiciousFactDetected @event) 
        {
            Id = @event.Id;
        }
    }
}