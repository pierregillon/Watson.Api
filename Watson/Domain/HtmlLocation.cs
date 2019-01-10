using System;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Domain
{
    public struct HtmlLocation
    {
        public string FirstNodeXPath { get; set; }
        public string LastNodeXPath { get; set; }
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
    }
}