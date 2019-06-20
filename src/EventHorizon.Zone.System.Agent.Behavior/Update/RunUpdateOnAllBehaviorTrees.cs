using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Agent.Behavior.Update
{
    public struct RunUpdateOnAllBehaviorTrees : INotification
    {
        public class RunUpdateOnAllBehaviorTreesHandler : INotificationHandler<RunUpdateOnAllBehaviorTrees>
        {
            readonly ILogger _logger;
            readonly ActorBehaviorTreeRepository _repository;
            readonly IServiceScopeFactory _serviceScopeFactory;

            public RunUpdateOnAllBehaviorTreesHandler(
                ILogger<RunUpdateOnAllBehaviorTreesHandler> logger,
                ActorBehaviorTreeRepository repository,
                IServiceScopeFactory serviceScopeFactory
            )
            {
                this._logger = logger;
                this._repository = repository;
                this._serviceScopeFactory = serviceScopeFactory;
            }

            public Task Handle(RunUpdateOnAllBehaviorTrees request, CancellationToken cancellationToken)
            {
                var treeIdList = _repository.TreeIdList();
                if (treeIdList.Count() > 0)
                {
                    Parallel.ForEach(treeIdList, async (treeId) =>
                    {
                        using (var serviceScope = _serviceScopeFactory.CreateScope())
                        {
                            var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                            try
                            {
                                await mediator.Publish(new RunBehaviorTreeUpdate(
                                    treeId
                                )).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "{TreeId} failed Run.", treeId);
                            }
                        }
                    });
                }
                return Task.CompletedTask;
            }
        }
    }
}