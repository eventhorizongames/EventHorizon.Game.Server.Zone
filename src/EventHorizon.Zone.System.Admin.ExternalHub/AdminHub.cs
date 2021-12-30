namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Identity.Policies;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize(UserIdOrClientIdOrAdminPolicy.PolicyName)]
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
