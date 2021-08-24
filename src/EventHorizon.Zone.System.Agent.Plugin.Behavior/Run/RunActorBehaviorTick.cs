namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;

    using MediatR;

    public struct RunActorBehaviorTick : IRequest
    {
        public ActorBehaviorTick ActorBehaviorTick { get; }

        public RunActorBehaviorTick(
            ActorBehaviorTick actorBehaviorTick
        )
        {
            ActorBehaviorTick = actorBehaviorTick;
        }
    }
}
