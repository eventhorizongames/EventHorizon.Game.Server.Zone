namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    using global::System;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize("UserIdOrAdmin")]
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