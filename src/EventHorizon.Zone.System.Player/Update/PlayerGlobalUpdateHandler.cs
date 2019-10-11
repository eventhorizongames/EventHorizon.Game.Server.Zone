using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Events.Update;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Update.Handler
{
    public class PlayerGlobalUpdateHandler : INotificationHandler<PlayerGlobalUpdateEvent>
    {
        readonly PlayerServerConnectionFactory _connectionFactory;
        public PlayerGlobalUpdateHandler(
            PlayerServerConnectionFactory connectionFactory
        )
        {
            _connectionFactory = connectionFactory;
        }
        public async Task Handle(
            PlayerGlobalUpdateEvent notification, 
            CancellationToken cancellationToken
        )
        {
            // TODO: Add palyer to a future to be updated queue, right now is currently setup to run synchronize when who called it.
            var connection = await _connectionFactory.GetConnection();
            await connection.SendAction(
                "UpdatePlayer", 
                PlayerFromEntityToDetails.Map(
                    notification.Player
                )
            );
        }
    }
}