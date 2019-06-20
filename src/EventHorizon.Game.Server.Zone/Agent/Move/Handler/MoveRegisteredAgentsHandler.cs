using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class MoveRegisteredAgentsHandler : INotificationHandler<MoveRegisteredAgentsEvent>
    {
        readonly ILogger _logger;
        readonly IMoveAgentRepository _moveRepository;
        readonly IServiceScopeFactory _serviceScopeFactory;
        public MoveRegisteredAgentsHandler(
            ILogger<MoveRegisteredAgentsHandler> logger,
            IMoveAgentRepository moveRepository,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _moveRepository = moveRepository;

            _serviceScopeFactory = serviceScopeFactory;
        }
        public Task Handle(MoveRegisteredAgentsEvent notification, CancellationToken cancellationToken)
        {
            // PERF: Could be a problem in the future with a lot of Agents
            // Solution: Move Agent processing to Shards/Partitioned Servers/Tasks of Agents
            var entityIdList = _moveRepository.All();
            if (entityIdList.Count() > 0)
            {
                if (entityIdList.Count() > 75)
                {
                    _logger.LogWarning("Agent Movement List is over 75.");
                }
                Parallel.ForEach(entityIdList, async (entityId) =>
                {
                    using (var serviceScope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                        try
                        {
                            await mediator.Publish(new MoveRegisteredAgentEvent
                            {
                                EntityId = entityId
                            }).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "{EntityId} failed to Move.", entityId);
                            _moveRepository.Remove(entityId);
                        }
                    }
                });
            }

            return Task.CompletedTask;
        }
    }
}