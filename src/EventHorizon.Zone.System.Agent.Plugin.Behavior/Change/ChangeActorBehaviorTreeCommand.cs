namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Change
{
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;

    public struct ChangeActorBehaviorTreeCommand : IRequest<bool>
    {
        public IObjectEntity Entity { get; }
        public string NewBehaviorTreeShapeId { get; }

        public ChangeActorBehaviorTreeCommand(
            IObjectEntity entity,
            string newBehaviorTreeShapeId
        )
        {
            Entity = entity;
            NewBehaviorTreeShapeId = newBehaviorTreeShapeId;
        }
    }
}