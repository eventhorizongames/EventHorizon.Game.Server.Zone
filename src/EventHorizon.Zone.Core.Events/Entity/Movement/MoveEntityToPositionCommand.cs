namespace EventHorizon.Zone.Core.Events.Entity.Movement
{
    using System.Numerics;

    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct MoveEntityToPositionCommand : IRequest<MoveEntityToPositionCommandResponse>
    {
        public IObjectEntity Entity { get; }
        public Vector3 MoveTo { get; }
        public bool DoDensityCheck { get; }

        public MoveEntityToPositionCommand(
            IObjectEntity entity,
            Vector3 moveTo,
            bool doDensityCheck
        )
        {
            Entity = entity;
            MoveTo = moveTo;
            DoDensityCheck = doDensityCheck;
        }
    }

    public struct MoveEntityToPositionCommandResponse
    {
        public bool Success { get; }
        public string Reason { get; }

        public MoveEntityToPositionCommandResponse(
            bool success,
            string reason
        )
        {
            Success = success;
            Reason = reason;
        }

        public MoveEntityToPositionCommandResponse(
            bool success
        )
        {
            Success = success;
            Reason = string.Empty;
        }
    }
}
