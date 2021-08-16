namespace EventHorizon.Zone.Core.Model.Entity.Client
{
    using EventHorizon.Zone.Core.Model.Client;
    using EventHorizon.Zone.Core.Model.Entity;

    public struct EntityChangedData : IClientActionData
    {
        public IObjectEntity Details { get; }

        public EntityChangedData(
            IObjectEntity entity
        )
        {
            Details = entity;
        }
    }
}
