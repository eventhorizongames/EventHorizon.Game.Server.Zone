namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Update
{
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class RunBehaviorTreeUpdateHandler : INotificationHandler<RunBehaviorTreeUpdate>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ActorBehaviorTreeRepository _repository;
        private readonly BehaviorInterpreterKernel _kernel;

        public RunBehaviorTreeUpdateHandler(
            ILogger<RunBehaviorTreeUpdateHandler> logger,
            IMediator mediator,
            ActorBehaviorTreeRepository repository,
            BehaviorInterpreterKernel kernel
        )
        {
            _logger = logger;
            _mediator = mediator;
            _repository = repository;
            _kernel = kernel;
        }

        public async Task Handle(
            RunBehaviorTreeUpdate notification,
            CancellationToken cancellationToken
        )
        {
            var behaviorTreeActorIdList = _repository.ActorIdList(
                notification.TreeId
            );
            var behaviorTreeShape = _repository.FindTreeShape(
                notification.TreeId
            );
            if (!behaviorTreeShape.IsValid || behaviorTreeShape.NodeList.Count == 0)
            {
                _logger.LogWarning(
                    "Invalid or Empty Behavior Tree Shape \n | BehaviorTreeShapeId: {BehaviorTreeShapeId}",
                    notification.TreeId,
                    notification,
                    behaviorTreeShape
                );
                // TODO: Monitor: Publish Event of invalid BehaviorTreeShapeId.
                // Handler should pick up, and keep track of how many times this behavior tree used and invalid.
                // After a threshold remove the tree from the respoitory and log an error.
                _repository.RemoveTreeShape(
                    notification.TreeId
                );
                return;
            }
            foreach (var actorId in behaviorTreeActorIdList)
            {
                await _kernel.Tick(
                    behaviorTreeShape,
                    await _mediator.Send(
                        new GetEntityByIdEvent
                        {
                            EntityId = actorId
                        }
                    )
                );
            }
        }
    }
}