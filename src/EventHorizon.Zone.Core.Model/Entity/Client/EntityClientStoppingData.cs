namespace EventHorizon.Zone.Core.Model.Entity.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    public struct EntityClientStoppingData : IClientActionData
    {
        public long EntityId { get; set; }
    }
}