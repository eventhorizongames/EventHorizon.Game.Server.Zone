using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Connection;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Loop.State;
using EventHorizon.Game.Server.Zone.Player.Connection;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Update.Handler
{
    public class PlayerGlobalUpdateHandler : INotificationHandler<PlayerGlobalUpdateEvent>
    {
        readonly IPlayerConnectionFactory _connectionFactory;
        public PlayerGlobalUpdateHandler(IPlayerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task Handle(PlayerGlobalUpdateEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Add palyer to a future to be updated queue, right now is currently setup to run synchronize when who called it.
            var connection = await _connectionFactory.GetConnection();
            await connection.UpdatePlayer(
                PlayerFromEntityToDetails.Map(notification.Player)
            );
        }
    }
}