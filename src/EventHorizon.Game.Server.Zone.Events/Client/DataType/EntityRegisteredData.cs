using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityRegisteredData : IClientActionData
    {
        public IObjectEntity Entity { get; set; }
    }
}