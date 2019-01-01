using System;
using System.Collections.Generic;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class WebPage : AggregateRoot
    {
        public override Guid Id => Guid.NewGuid();

        private List<FalseInformation> _falseInformations = new List<FalseInformation>();

        public void Report(FalseInformation falseInformation)
        {
            ApplyChange(new FalseInformationReported(falseInformation));
        }

        public void Apply(FalseInformationReported falseInformationReported)
        {
            _falseInformations.Add(falseInformationReported.FalseInformation);
        }
    }
}