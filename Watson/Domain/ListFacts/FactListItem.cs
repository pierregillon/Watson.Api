using System;

namespace Watson.Domain.ListFacts
{
    public class FactListItem
    {
        public string WebPageUrl { get; set; }
        public string Wording { get; set; }
        public string StartNodeXPath { get; set; }
        public string EndNodeXPath { get; set; }
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
    }
}