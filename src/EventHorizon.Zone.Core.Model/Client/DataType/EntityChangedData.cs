using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.Core.Model.Client.DataType
{
    public struct EntityChangedData : IClientActionData
    {
        public IObjectEntity Details { get; }
        public EntityChangedData(
            IObjectEntity entity
        ) {
            Details = entity;
        }
    }
}