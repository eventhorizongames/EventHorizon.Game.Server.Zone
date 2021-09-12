namespace EventHorizon.Zone.System.Admin.AdminClientAction
{
    using EventHorizon.Zone.System.Admin.AdminClientAction.Client;
    using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class AdminClientActionToAllHandler<T, J>
        where T : AdminClientActionToAllEvent<J>
        where J : IAdminClientActionData
    {
        private readonly IMediator _mediator;

        public AdminClientActionToAllHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            T notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                new SendToAllAdminClientsEvent
                {
                    Method = "AdminClientAction",
                    Arg1 = notification.Action,
                    Arg2 = notification.Data
                },
                cancellationToken
            );
        }
    }
}
