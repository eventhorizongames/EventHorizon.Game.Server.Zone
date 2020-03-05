namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Change
{
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;

    public class ChangeActorBehaviorTreeCommand : IRequest<bool>
    {
        public IObjectEntity Entity { get; }
        public string NewBehaviorTreeId { get; }

        public ChangeActorBehaviorTreeCommand(
            IObjectEntity entity,
            string newBehaviorTreeId
        )
        {
            Entity = entity;
            NewBehaviorTreeId = newBehaviorTreeId;
        }
    }
}