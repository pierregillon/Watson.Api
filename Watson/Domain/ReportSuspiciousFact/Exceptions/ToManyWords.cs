namespace Watson.Domain.ReportSuspiciousFact
{
    public class ToManyWords : DomainException
    {
        public ToManyWords(int maximumCount) : base($"A new suspicious fact wording must contain no more than {maximumCount} words.") { }
    }
}