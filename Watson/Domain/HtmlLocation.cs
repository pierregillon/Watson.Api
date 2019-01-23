using System;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Domain
{
    public class HtmlLocation
    {
        public XPath StartNodeXPath { get; set; }
        public XPath EndNodeXPath { get; set; }
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
    }
}