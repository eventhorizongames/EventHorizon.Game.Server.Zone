using System.Numerics;

namespace EventHorizon.Zone.Core.Model.Client.DataType
{
    public struct EntityClientMoveData : IClientActionData
    {
        public long EntityId { get; set; }
        public Vector3 MoveTo { get; set; }
    }
}