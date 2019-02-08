namespace Watson.Domain.ReportSuspiciousFact
{
    public class FactSpreadOverMultipleParagraphs : DomainException
    {
        public FactSpreadOverMultipleParagraphs() : base("Cannot create fact that is spread over multiple paragraphs.") {}
    }
}