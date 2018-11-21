using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.External.Client
{
    public class ClientActionToAllHandler<T, J> where T : ClientActionToAllEvent<J> where J : IClientActionData
    {
        readonly IMediator _mediator;
        public ClientActionToAllHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(T notification, CancellationToken cancellationToken)
        {
            return _mediator.Publish(
                new SendToAllClientsEvent
                {
                    Method = "ClientAction",
                    Arg1 = notification.Action,
                    Arg2 = notification.Data
                }
            );
        }
    }
}