using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Update
{
    public struct RunUpdateOnAllBehaviorTrees : INotification
    {
        public class RunUpdateOnAllBehaviorTreesHandler : INotificationHandler<RunUpdateOnAllBehaviorTrees>
        {
            readonly ILogger _logger;
            readonly IMediator _mediator;
            readonly ActorBehaviorTreeRepository _repository;
            readonly IServiceScopeFactory _serviceScopeFactory;

            public RunUpdateOnAllBehaviorTreesHandler(
                ILogger<RunUpdateOnAllBehaviorTreesHandler> logger,
                IMediator mediator,
                ActorBehaviorTreeRepository repository,
                IServiceScopeFactory serviceScopeFactory
            )
            {
                this._logger = logger;
                _mediator = mediator;
                this._repository = repository;
                this._serviceScopeFactory = serviceScopeFactory;
            }

            public Task Handle(RunUpdateOnAllBehaviorTrees request, CancellationToken cancellationToken)
            {
                var treeIdList = _repository.TreeIdList();
                if (treeIdList.Count() > 0)
                {
                    Task.WaitAll(
                        treeIdList.Select(
                            treeId => RunBehaviorTreeUpdateByTreeId(
                                treeId
                            )
                        ).ToArray()
                    );
                }
                return Task.CompletedTask;
            }

            private async Task RunBehaviorTreeUpdateByTreeId(
                string treeId
            )
            {
                try
                {
                    await _mediator.Publish(new RunBehaviorTreeUpdate(
                        treeId
                    ));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{TreeId} failed Run.", treeId);
                }
            }
        }
    }
}