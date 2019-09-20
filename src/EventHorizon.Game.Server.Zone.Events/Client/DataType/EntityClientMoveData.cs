using System.Numerics;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Zone.Core.Model.Client;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityClientMoveData : IClientActionData
    {
        public long EntityId { get; set; }
        public Vector3 MoveTo { get; set; }
    }
}