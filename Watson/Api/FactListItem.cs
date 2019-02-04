using System;

namespace Watson.Api
{
    public class FactListItem
    {
        public string webPageUrl { get; internal set; }
        public string wording { get; internal set; }
        public string firstTextNodeXPath { get; internal set; }
        public string lastTextNodeXPath { get; internal set; }
        public int startOffset { get; internal set; }
        public int endOffset { get; internal set; }
    }
}