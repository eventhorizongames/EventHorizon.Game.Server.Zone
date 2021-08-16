namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;

    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class RunActorBehaviorTickHandler : IRequestHandler<RunActorBehaviorTick>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly BehaviorInterpreterKernel _kernel;
        private readonly ActorBehaviorTickQueue _queue;

        public RunActorBehaviorTickHandler(
            ILogger<RunActorBehaviorTickHandler> logger,
            IMediator mediator,
            BehaviorInterpreterKernel kernel,
            ActorBehaviorTickQueue queue
        )
        {
            _logger = logger;
            _mediator = mediator;
            _kernel = kernel;
            _queue = queue;
        }

        public async Task<Unit> Handle(
            RunActorBehaviorTick request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var response = await _mediator.Send(
                    new IsValidActorBehaviorTick(
                        request.ActorBehaviorTick
                    )
                );
                if (!response.IsValid)
                {
                    return Unit.Value;
                }
                var actor = response.Actor;
                var behaviorTreeShape = response.Shape;
                // Run Kernel Tick against valididated Shape and Actor 
                var result = await _kernel.Tick(
                    behaviorTreeShape,
                    actor
                );
                await _mediator.Send(
                    new PostProcessActorBehaviorTickResult(
                        result,
                        request.ActorBehaviorTick,
                        actor,
                        behaviorTreeShape
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "General Exception Running Kernel Tick \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId}",
                    request.ActorBehaviorTick.ShapeId,
                    request.ActorBehaviorTick.ActorId,
                    request
                );
                _queue.RegisterFailed(
                    request.ActorBehaviorTick
                );
            }

            return Unit.Value;
        }
    }
}
