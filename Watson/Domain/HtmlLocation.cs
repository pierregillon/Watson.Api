using System;
using Newtonsoft.Json;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Domain
{
    public class HtmlLocation
    {
        [JsonConverter(typeof(ObjectToStringConverter))]
        public XPath StartNodeXPath { get; set; }
        
        [JsonConverter(typeof(ObjectToStringConverter))]
        public XPath EndNodeXPath { get; set; }
        
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
    }
}