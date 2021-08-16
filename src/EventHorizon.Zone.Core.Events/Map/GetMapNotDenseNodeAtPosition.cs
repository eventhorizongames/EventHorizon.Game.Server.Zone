namespace EventHorizon.Zone.Core.Events.Map
{
    using System.Numerics;

    using EventHorizon.Zone.Core.Model.Map;

    using MediatR;

    public struct GetMapNotDenseNodeAtPosition : IRequest<MapNode>
    {
        public Vector3 Position { get; }

        public GetMapNotDenseNodeAtPosition(
            Vector3 position
        )
        {
            Position = position;
        }
    }
}
