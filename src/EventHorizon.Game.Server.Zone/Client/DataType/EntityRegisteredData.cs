using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityRegisteredData : IClientActionData
    {
        public IObjectEntity Entity { get; set; }
    }
}