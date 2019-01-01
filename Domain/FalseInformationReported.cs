using System;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class FalseInformationReported : Event
    {
        public readonly Guid Id;
        public readonly FalseInformation FalseInformation;

        public FalseInformationReported(Guid id, FalseInformation falseInformation)
        {
            Id = id;
            FalseInformation = falseInformation;
        }
    }
}