namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize("UserIdOrClientIdOrAdmin")]
    public partial class AdminHub : Hub
    {
        readonly IMediator _mediator;

        public AdminHub(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
    }
}
