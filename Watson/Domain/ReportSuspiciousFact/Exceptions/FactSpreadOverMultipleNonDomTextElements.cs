using System;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class FactSpreadOverMultipleNonDomTextElements : Exception
    {
        public FactSpreadOverMultipleNonDomTextElements() : base("Cannot create fact that is spread over multiple non dom text elements.") {}
    }
}