using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Move.Finished
{
    public class AgentFinishedMoveEventHandler : INotificationHandler<AgentFinishedMoveEvent>
    {
        private readonly IMediator _mediator;

        public AgentFinishedMoveEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public Task Handle(
            AgentFinishedMoveEvent notification, 
            CancellationToken cancellationToken
        )
        {
            return _mediator.Publish(
                new ClientActionClientEntityStoppingToAllEvent
                {
                    Data = new EntityClientStoppingData
                    {
                        EntityId = notification.EntityId,
                    },
                }
            );
        }
    }
}