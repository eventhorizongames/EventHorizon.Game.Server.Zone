namespace EventHorizon.Zone.System.ClientEntities.Unregister
{
    using MediatR;

    public class UnregisterClientEntity : IRequest<bool>
    {
        public string GlobalId { get; }

        public UnregisterClientEntity(
            string globalId
        )
        {
            GlobalId = globalId;
        }
    }
}
