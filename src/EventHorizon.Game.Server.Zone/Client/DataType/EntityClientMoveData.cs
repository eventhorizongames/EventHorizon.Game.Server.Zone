using System.Numerics;
using EventHorizon.Game.Server.Zone.Client;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityClientMoveData : IClientActionData
    {
        public long EntityId { get; set; }
        public Vector3 MoveTo { get; set; }
    }
}