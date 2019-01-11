using System;
using System.Linq;
using System.Text.RegularExpressions;
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

            CheckFormat(location.FirstNodeXPath);
            CheckFormat(location.LastNodeXPath);

            for (int i = 0; i < Math.Max(location.FirstNodeXPath.Length, location.LastNodeXPath.Length); i++) {
                var node1 = FindNode(location.FirstNodeXPath, i);
                var node2 = FindNode(location.LastNodeXPath, i);
                if(node1 != node2) {
                    if(new [] {node1, node2}.Any(x=>new [] { "strong", "a", "span", "text()" }.Any(y=>x == null || x.StartsWith(y)) == false)) {
                        throw new FactSpreadOverMultipleNonDomTextElements();
                    }
                }
            }
            
            ApplyChange(new SuspiciousFactDetected(Guid.NewGuid(), wording, webPageUrl, location));
        }

        private void CheckFormat(string xPath)
        {
            if(string.IsNullOrEmpty(xPath)) {
                throw new InvalidXPathFormat("Invalid xPath format.");
            }
            
            var regex = new Regex(@"^(?<id>\/\/\*\[\@id=\'.*\'\])?(?<path>\/[a-z]*(\[\d*\])?)*\/text\(\)(\[\d*\])?$".Replace("\'", "\""));
            if(regex.IsMatch(xPath) == false) {
                throw new InvalidXPathFormat("Invalid xPath format.");
            }
        }

        private string FindNode(string xPath, int index) {
            var pathes = xPath.Split('/');
            if(index < pathes.Length) {
                return pathes[index];
            }
            return null;
        }

        private void Apply(SuspiciousFactDetected @event) 
        {
            Id = @event.Id;
        }
    }
}