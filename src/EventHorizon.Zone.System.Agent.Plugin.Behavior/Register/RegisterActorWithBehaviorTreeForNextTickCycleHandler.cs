namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class RegisterActorWithBehaviorTreeForNextTickCycleHandler
        : IRequestHandler<RegisterActorWithBehaviorTreeForNextTickCycle>
    {
        private readonly ActorBehaviorTickQueue _queue;

        public RegisterActorWithBehaviorTreeForNextTickCycleHandler(ActorBehaviorTickQueue queue)
        {
            _queue = queue;
        }

        public Task Handle(
            RegisterActorWithBehaviorTreeForNextTickCycle request,
            CancellationToken cancellationToken
        )
        {
            _queue.Register(request.ShapeId, request.ActorId);

            return Task.CompletedTask;
        }
    }
}
