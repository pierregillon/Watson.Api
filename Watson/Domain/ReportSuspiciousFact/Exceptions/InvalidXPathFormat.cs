namespace Watson.Domain.ReportSuspiciousFact
{
    public class InvalidXPathFormat : DomainException
    {
        public InvalidXPathFormat() : base("Invalid xPath format.")
        {
        }
    }
}