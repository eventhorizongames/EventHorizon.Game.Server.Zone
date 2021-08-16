namespace EventHorizon.Zone.Core.Model.Entity.Client
{
    using System.Numerics;

    using EventHorizon.Zone.Core.Model.Client;

    public struct EntityClientMoveData : IClientActionData
    {
        public long EntityId { get; set; }
        public Vector3 MoveTo { get; set; }
    }
}
