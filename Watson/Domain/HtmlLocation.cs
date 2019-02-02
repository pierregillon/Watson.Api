using System;
using Newtonsoft.Json;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Domain
{
    public class HtmlLocation
    {
        public string StartNodeXPath { get; set; }
        public string EndNodeXPath { get; set; }
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
    }
}