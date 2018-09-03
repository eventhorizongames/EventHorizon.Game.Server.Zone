using EventHorizon.Game.Server.Zone.Entity.Model;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityRegisteredData : IClientActionData
    {
        public IObjectEntity Entity { get; set; }
    }
}