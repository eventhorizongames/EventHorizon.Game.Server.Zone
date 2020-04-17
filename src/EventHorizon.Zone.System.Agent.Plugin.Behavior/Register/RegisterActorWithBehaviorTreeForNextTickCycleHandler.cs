namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
    using MediatR;

    public class RegisterActorWithBehaviorTreeForNextTickCycleHandler : IRequestHandler<RegisterActorWithBehaviorTreeForNextTickCycle>
    {
        private readonly ActorBehaviorTickQueue _queue;

        public RegisterActorWithBehaviorTreeForNextTickCycleHandler(
            ActorBehaviorTickQueue queue
        )
        {
            _queue = queue;
        }

        public Task<Unit> Handle(
            RegisterActorWithBehaviorTreeForNextTickCycle request,
            CancellationToken cancellationToken
        )
        {
            _queue.Register(
                request.ShapeId,
                request.ActorId
            );
            return Unit.Task;
        }
    }
}