using System.Numerics;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Model.Client;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityClientStoppingData : IClientActionData
    {
        public long EntityId { get; set; }
    }
}