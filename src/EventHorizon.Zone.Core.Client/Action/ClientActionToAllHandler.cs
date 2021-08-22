namespace EventHorizon.Zone.Core.Client.Action
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Client;
    using EventHorizon.Zone.Core.Model.Client;

    using MediatR;

    public class ClientActionToAllHandler<T, J> where T : ClientActionToAllEvent<J> where J : IClientActionData
    {
        readonly IMediator _mediator;

        public ClientActionToAllHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public Task Handle(
            T notification,
            CancellationToken cancellationToken
        ) => _mediator.Publish(
            new SendToAllClientsEvent
            {
                Method = "ClientAction",
                Arg1 = notification.Action,
                Arg2 = notification.Data
            }
        );
    }
}
