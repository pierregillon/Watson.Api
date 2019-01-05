using System;
using Watson.Domain.SuspectFalseFact;

namespace Watson.Domain
{
    public struct HtmlLocation
    {
        public string FirstNodeXPath { get; set; }
        public string LastNodeXPath { get; set; }
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }

        public HtmlLocation(string firstNodeXPath, string lastNodeXPath, int startOffset, int endOffset)
        {
            FirstNodeXPath = firstNodeXPath;
            LastNodeXPath = lastNodeXPath;
            StartOffset = startOffset;
            EndOffset = endOffset;
        }

        public void AssertValid(string wording)
        {
            if (string.IsNullOrEmpty(FirstNodeXPath) || string.IsNullOrEmpty(LastNodeXPath)) {
                throw new InvalidHtmlLocation("Both node Xmap should be defined.");
            }
            if(StartOffset < 0 || EndOffset < 0 || StartOffset >= EndOffset) {
                throw new InvalidHtmlLocationOffsets();
            }
            if (wording.Length != EndOffset - StartOffset) {
                throw new FactWordingAndOffsetInconsistent();
            }
        }
    }
}