namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
    using MediatR;

    public struct IsValidActorBehaviorTick : IRequest<ActorBehaviorTickValidationResponse>
    {
        public ActorBehaviorTick ActorBehaviorTick { get; }

        public IsValidActorBehaviorTick(
            ActorBehaviorTick actorBehaviorTick
        )
        {
            this.ActorBehaviorTick = actorBehaviorTick;
        }
    }

    public struct ActorBehaviorTickValidationResponse
    {
        public bool IsValid { get; }
        public IObjectEntity Actor { get; }
        public ActorBehaviorTreeShape Shape { get; }

        public ActorBehaviorTickValidationResponse(
            IObjectEntity actor,
            ActorBehaviorTreeShape shape
        )
        {
            IsValid = actor != null 
                && actor.IsFound() 
                && shape.IsValid 
                && shape.NodeList.Count > 0;
            Actor = actor;
            Shape = shape;
        }
    }
}