namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;

using EventHorizon.Performance;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class RunPendingActorBehaviorTicksHandler : INotificationHandler<RunPendingActorBehaviorTicks>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    private readonly ActorBehaviorTickQueue _queue;
    private readonly PerformanceTrackerFactory _performanceTrackerFactory;

    public RunPendingActorBehaviorTicksHandler(
        ILogger<RunPendingActorBehaviorTicksHandler> logger,
        IMediator mediator,
        ActorBehaviorTickQueue queue,
        PerformanceTrackerFactory performanceTrackerFactory
    )
    {
        _logger = logger;
        _mediator = mediator;
        _queue = queue;
        _performanceTrackerFactory = performanceTrackerFactory;
    }

    public Task Handle(
        RunPendingActorBehaviorTicks notification,
        CancellationToken cancellationToken
    )
    {
        _queue.PrimeQueueWithRegisteredTicks();
        var runCount = 0;
        while (_queue.Dequeue(out var actorBehaviorTick))
        {
            using (_performanceTrackerFactory.Build("RunActorBehaviorTick"))
            {
                _ = _mediator.Send(
                   new RunActorBehaviorTick(
                       actorBehaviorTick
                   )
               );
            }
            // TODO: Replace this with a timer,
            //  should only run for as many as it can in a certain amount of time.
            // FUTURE-CIRCUIT: Look at triggering an circuit
            // The circuit should keep tabs on how many times this is triggered and if
            //  a certain threshold is reached do something to fixed the to many agents running warnings.
            runCount++;
        }
        // if (runCount > 10)
        // {
        //     _logger.LogWarning("ActorBehaviorTick Count is over 10.");
        // }
        if (runCount > 100)
        {
            _logger.LogWarning("ActorBehaviorTick Count is over 100.");
            _logger.LogWarning("RunActorBehaviorTick Count is {Count}.", runCount);
        }
        return Task.CompletedTask;
    }
}
