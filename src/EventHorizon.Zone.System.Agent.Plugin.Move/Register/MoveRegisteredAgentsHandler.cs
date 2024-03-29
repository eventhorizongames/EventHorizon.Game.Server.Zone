namespace EventHorizon.Zone.System.Agent.Move.Register;

using EventHorizon.Performance;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class MoveRegisteredAgentsHandler
    : INotificationHandler<MoveRegisteredAgentsEvent>
{
    private readonly IMediator _mediator;
    private readonly IMoveAgentRepository _moveRepository;
    private readonly PerformanceTrackerFactory _performanceTrackerFactory;

    public MoveRegisteredAgentsHandler(
        IMediator mediator,
        IMoveAgentRepository moveRepository,
        PerformanceTrackerFactory performanceTrackerFactory
    )
    {
        _mediator = mediator;
        _moveRepository = moveRepository;
        _performanceTrackerFactory = performanceTrackerFactory;
    }

    public Task Handle(
        MoveRegisteredAgentsEvent notification,
        CancellationToken cancellationToken
    )
    {
        _moveRepository.MergeRegisteredIntoQueue();
        // TODO: PERF: Could be a problem in the future with a lot of Agents
        // Solution: Move Agent processing to Shards/Partitioned Servers/Tasks of Agents
        var runCount = 0;
        while (_moveRepository.Dequeue(out var entityId))
        {
            // TODO: PERF: Work on getting this below 5ms average, currently 5-10ms
            using (_performanceTrackerFactory.Build("MoveRegisteredAgentEvent"))
            {
                _ = _mediator.Publish(
                    new MoveRegisteredAgentEvent(
                        entityId
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
        //     _logger.LogWarning("MoveRegisteredAgentEvent Count is over 10.");
        // }
        // if (runCount > 100)
        // {
        //     _logger.LogWarning("MoveRegisteredAgentEvent Count is over 100.");
        //     _logger.LogWarning("MoveRegisteredAgentEvent Count is {Count}.", runCount);
        // }
        return Task.CompletedTask;
    }
}
