using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.Core.Model.Client.DataType
{
    public struct EntityRegisteredData : IClientActionData
    {
        public IObjectEntity Entity { get; set; }
    }
}