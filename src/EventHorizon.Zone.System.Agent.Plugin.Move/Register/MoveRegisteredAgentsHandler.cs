namespace EventHorizon.Zone.System.Agent.Move.Register
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Performance;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

    public class MoveRegisteredAgentsHandler : INotificationHandler<MoveRegisteredAgentsEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IMoveAgentRepository _moveRepository;
        readonly IPerformanceTracker _performanceTracker;

        public MoveRegisteredAgentsHandler(
            ILogger<MoveRegisteredAgentsHandler> logger,
            IMediator mediator,
            IMoveAgentRepository moveRepository,
            IPerformanceTracker performanceTracker
        )
        {
            _logger = logger;
            _mediator = mediator;
            _moveRepository = moveRepository;
            _performanceTracker = performanceTracker;
        }

        public Task Handle(
            MoveRegisteredAgentsEvent notification,
            CancellationToken cancellationToken
        )
        {
            _moveRepository.MergeRegisteredIntoQueue();
            // PERF: Could be a problem in the future with a lot of Agents
            // Solution: Move Agent processing to Shards/Partitioned Servers/Tasks of Agents
            var entityCount = 0;
            while (_moveRepository.Dequeue(out var entityId))
            {
                // TODO: PERF: Work on getting this below 5ms average, currently 5-10ms
                using (_performanceTracker.Track("MoveRegisteredAgentEvent"))
                {
                    _ = _mediator.Publish(
                        new MoveRegisteredAgentEvent
                        {
                            EntityId = entityId
                        }
                    );
                }
                entityCount++;
                if (entityCount > 10)
                {
                    _logger.LogWarning("Agent Movement List is over 10.");
                }
                if (entityCount > 25)
                {
                    _logger.LogWarning("Agent Movement List is over 25.");
                    // FUTURE-CIRCUIT: Look at triggering an circuit
                    // The circuit should keep tabs on how many times this is triggered and if
                    //  a certain threshold is reached do something to fixed the to many agents running warnings.
                    // break;
                }
            }
            return Task.CompletedTask;
        }
    }
}