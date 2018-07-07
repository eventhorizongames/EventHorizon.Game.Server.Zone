using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Player.Events.Details;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Player.Actions;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using EventHorizon.Game.Server.Zone.Player.Model;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.Update;
using EventHorizon.Game.Server.Zone.Player.Zone;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Player.Connected.Handler
{
    public class PlayerConnectedHandler : INotificationHandler<PlayerConnectedEvent>
    {
        readonly ILogger _logger;
        readonly IServerProperty _serverProperty;
        readonly IMediator _mediator;
        readonly IPlayerRepository _player;

        public PlayerConnectedHandler(ILogger<PlayerConnectedHandler> logger, IServerProperty serverProperty, IMediator mediator, IEntityRepository entityRepository, IPlayerRepository player)
        {
            _logger = logger;
            _serverProperty = serverProperty;
            _mediator = mediator;
            _player = player;
        }
        public async Task Handle(PlayerConnectedEvent notification, CancellationToken cancellationToken)
        {
            // Check for player on this zone server
            var player = await _player.FindById(notification.Id);
            if (player.Equals(PlayerEntity.NULL))
            {
                try
                {
                    var globalPlayer = await _mediator.Send(new PlayerGetDetailsEvent
                    {
                        Id = notification.Id
                    });
                    if (!globalPlayer.Position
                        .CurrentZone.Equals(_serverProperty.Get<string>(ServerPropertyKeys.SERVER_ID)))
                    {
                        throw new Exception("Player is not part of this server.");
                    }
                    // Create new Player
                    globalPlayer.Data.ConnectionId = notification.ConnectionId;
                    player = (Model.PlayerEntity)await _mediator.Send(new RegisterEntityEvent
                    {
                        Entity = PlayerFromDetailsToEntity.MapToNew(globalPlayer),
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                    throw ex;
                }
            }

            // Update players ConnectionId
            player.ConnectionId = notification.ConnectionId;
            await _player.Update(player);
            await _mediator.Publish(new PlayerGlobalUpdateEvent
            {
                Player = player,
            });

            await _mediator.Send(new SendZoneInfoToPlayerEvent
            {
                Player = player
            });
        }
    }
}