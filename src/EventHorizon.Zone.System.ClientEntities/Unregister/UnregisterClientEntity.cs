namespace EventHorizon.Zone.System.ClientEntities.Unregister
{
    using MediatR;

    public class UnregisterClientEntity : IRequest<bool>
    {
        public string Id { get; }

        public UnregisterClientEntity(
            string id
        )
        {
            Id = id;
        }
    }
}