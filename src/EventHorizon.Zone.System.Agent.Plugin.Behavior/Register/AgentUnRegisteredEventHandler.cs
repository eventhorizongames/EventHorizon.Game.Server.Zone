using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
{
    public class AgentUnRegisteredEventHandler : INotificationHandler<AgentUnRegisteredEvent>
    {
        readonly ActorBehaviorTreeRepository _repository;
        public AgentUnRegisteredEventHandler(
            ActorBehaviorTreeRepository repository
        )
        {
            _repository = repository;
        }

        public Task Handle(
            AgentUnRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            _repository.UnRegisterActor(
                notification.EntityId
            );
            return Task.CompletedTask;
        }
    }
}