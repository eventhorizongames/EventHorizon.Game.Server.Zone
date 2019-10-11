using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Connection;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Zone.System.Player.Connection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Player.Events.Details.Handler
{
    public class PlayerGetDetailsHandler : IRequestHandler<PlayerGetDetailsEvent, PlayerDetails>
    {
        readonly PlayerServerConnectionFactory _connectionFactory;
        public PlayerGetDetailsHandler(
            PlayerServerConnectionFactory connectionFactory
        )
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PlayerDetails> Handle(
            PlayerGetDetailsEvent request,
            CancellationToken cancellationToken
        )
        {
            var connection = await _connectionFactory.GetConnection();
            var response = await connection.SendAction<PlayerDetails>(
                "GetPlayer",
                request.Id
            );
            return response;
        }
    }
}