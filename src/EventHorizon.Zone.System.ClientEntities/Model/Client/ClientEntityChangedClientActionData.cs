namespace EventHorizon.Zone.System.ClientEntities.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    public struct ClientEntityChangedClientActionData : IClientActionData
    {
        public ClientEntityDetails Details { get; }

        public ClientEntityChangedClientActionData(
            ClientEntityDetails details
        )
        {
            Details = details;
        }
    }
}