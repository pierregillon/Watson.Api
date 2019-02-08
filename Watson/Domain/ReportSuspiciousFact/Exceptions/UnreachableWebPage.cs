namespace Watson.Domain.ReportSuspiciousFact
{
    public class UnreachableWebPage : DomainException
    {
        public UnreachableWebPage(string url) : base($"The web page '{url}' is unreachable.") { }
    }
}