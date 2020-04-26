namespace EventHorizon.Zone.Core.Model.Entity.Client
{
    using EventHorizon.Zone.Core.Model.Client;
    using System.Numerics;

    public struct EntityClientMoveData : IClientActionData
    {
        public long EntityId { get; set; }
        public Vector3 MoveTo { get; set; }
    }
}