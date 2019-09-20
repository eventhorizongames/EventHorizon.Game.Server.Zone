using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using MediatR;

namespace EventHorizon.Zone.Core.Model.Client
{
    public class ClientActionToSingleHandler<T, J> where T : ClientActionToSingleEvent<J> where J : IClientActionData
    {
        readonly IMediator _mediator;
        public ClientActionToSingleHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(T notification, CancellationToken cancellationToken)
        {
            return _mediator.Publish(
                new SendToSingleClientEvent
                {
                    ConnectionId = notification.ConnectionId,
                    Method = "ClientAction",
                    Arg1 = notification.Action,
                    Arg2 = notification.Data
                }
            );
        }
    }
}