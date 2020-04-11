namespace EventHorizon.Zone.System.ClientEntities.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    public struct ClientEntityChangedClientActionData : IClientActionData
    {
        public ClientEntity Details { get; }

        public ClientEntityChangedClientActionData(
            ClientEntity details
        )
        {
            Details = details;
        }
    }
}