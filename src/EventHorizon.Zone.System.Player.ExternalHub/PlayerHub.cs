namespace EventHorizon.Zone.System.Player.ExternalHub;

using EventHorizon.Zone.System.Player.Events.Connected;

using global::System;
using global::System.Linq;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

[Authorize]
public partial class PlayerHub
    : Hub
{
    private readonly ILogger<PlayerHub> _logger;
    private readonly IMediator _mediator;

    public PlayerHub(
        ILogger<PlayerHub> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        var playerId = GetPlayerId();
        if (playerId == null)
        {
            Context.Abort();
            return;
        }

        _logger.LogDebug(
            "Player Connected: {PlayerId}",
            playerId
        );
        await _mediator.Publish(
            new PlayerConnectedEvent(
                playerId,
                Context.ConnectionId
            )
        );
    }

    public override async Task OnDisconnectedAsync(
        Exception exception
    )
    {
        var playerId = GetPlayerId();
        if (playerId == null)
        {
            return;
        }

        _logger.LogError(
            exception,
            "Player Disconnected: {PlayerId}",
            playerId
        );
        await _mediator.Publish(
            new PlayerDisconnectedEvent(
                playerId
            )
        );
    }

    private string? GetPlayerId()
    {
        return Context.User.Claims
            .FirstOrDefault(
                claim => claim.Type == "sub"
            )?.Value;
    }

    private string GetPlayerIdNotNull()
    {
        return Context.User.Claims
            .First(
                claim => claim.Type == "sub"
            ).Value;
    }
}
