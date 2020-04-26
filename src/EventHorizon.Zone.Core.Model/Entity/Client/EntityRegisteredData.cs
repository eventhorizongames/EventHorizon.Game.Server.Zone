namespace EventHorizon.Zone.Core.Model.Entity.Client
{
    using EventHorizon.Zone.Core.Model.Client;
    using EventHorizon.Zone.Core.Model.Entity;

    public struct EntityRegisteredData : IClientActionData
    {
        public IObjectEntity Entity { get; set; }
    }
}