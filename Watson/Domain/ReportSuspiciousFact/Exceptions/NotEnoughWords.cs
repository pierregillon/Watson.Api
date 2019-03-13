namespace Watson.Domain.ReportSuspiciousFact
{
    public class NotEnoughWords : DomainException
    {
        public NotEnoughWords(int minimumCount) : base($"A new suspicious fact wording must contain at least {minimumCount} words.")
        {
        }
    }
}