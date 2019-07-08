using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;

namespace EventHorizon.Zone.System.Agent.Behavior.Update
{
    public struct RunBehaviorTreeUpdate : INotification
    {
        public string TreeId { get; }
        public RunBehaviorTreeUpdate(
            string treeId
        ) => TreeId = treeId;

        public struct RunBehaviorTreeUpdateHandler : INotificationHandler<RunBehaviorTreeUpdate>
        {
            readonly IMediator _mediator;
            readonly ActorBehaviorTreeRepository _repository;
            readonly BehaviorInterpreterKernel _kernel;
            public RunBehaviorTreeUpdateHandler(
                IMediator mediator,
                ActorBehaviorTreeRepository repository,
                BehaviorInterpreterKernel kernel
            )
            {
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
                foreach (var actorId in behaviorTreeActorIdList)
                {
                    // TODO: Look at making this fire and forget.
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
}