using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class FalseInformationReported : Event
    {
        public readonly FalseInformation FalseInformation;

        public FalseInformationReported(FalseInformation falseInformation)
        {
            FalseInformation = falseInformation;
        }
    }
}