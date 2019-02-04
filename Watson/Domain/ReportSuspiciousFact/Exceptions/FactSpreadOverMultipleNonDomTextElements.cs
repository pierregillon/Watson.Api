using System;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class FactSpreadOverMultipleParagraphs : Exception
    {
        public FactSpreadOverMultipleParagraphs() : base("Cannot create fact that is spread over multiple paragraphs.") {}
    }
}