
using EventHorizon.Game.Server.Zone.Model.Client;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityUnregisteredData : IClientActionData
    {
        public long EntityId { get; set; }
    }
}