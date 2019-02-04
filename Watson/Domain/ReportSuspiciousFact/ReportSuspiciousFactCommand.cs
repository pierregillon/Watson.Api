using CQRSlite.Commands;
using Watson.Domain;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class ReportSuspiciousFactCommand : ICommand
    {
        public string Wording { get; set; }
        public string WebPageUrl { get; set; }
        public string StartNodeXPath { get; set; }
        public string EndNodeXPath { get; set; }
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
    }
}