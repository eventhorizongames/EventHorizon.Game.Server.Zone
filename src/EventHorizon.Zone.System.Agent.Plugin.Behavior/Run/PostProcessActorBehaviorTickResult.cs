namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
    using MediatR;

    public class PostProcessActorBehaviorTickResult : IRequest
    {
        public BehaviorTreeState Result { get; }
        public ActorBehaviorTick ActorBehaviorTick { get; }
        public IObjectEntity Actor { get; }
        public ActorBehaviorTreeShape Shape { get; }

        public PostProcessActorBehaviorTickResult(
            BehaviorTreeState result,
            ActorBehaviorTick actorBehaviorTick,
            IObjectEntity actor,
            ActorBehaviorTreeShape shape
        )
        {
            Result = result;
            ActorBehaviorTick = actorBehaviorTick;
            Actor = actor;
            Shape = shape;
        }
    }
}