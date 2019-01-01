using System;
using System.Collections.Generic;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class WebPage : AggregateRoot
    {
        public override Guid Id { get; protected set; }

        private List<FalseInformation> _falseInformations = new List<FalseInformation>();

        public WebPage()
        {

        }
        public WebPage(string url) : this()
        {
            ApplyChange(new WebPageRegistered(Guid.NewGuid(), url));
        }

        public void Report(FalseInformation falseInformation)
        {
            ApplyChange(new FalseInformationReported(Id, falseInformation));
        }

        private void Apply(FalseInformationReported falseInformationReported)
        {
            _falseInformations.Add(falseInformationReported.FalseInformation);
        }

        private void Apply(WebPageRegistered @event)
        {
            Id = @event.Id;
        }
    }
}