
namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityUnregisteredData : IClientActionData
    {
        public long EntityId { get; set; }
    }
}