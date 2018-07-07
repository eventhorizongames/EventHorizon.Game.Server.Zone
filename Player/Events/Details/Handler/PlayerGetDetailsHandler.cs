using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Connection;
using EventHorizon.Game.Server.Core.Player.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Player.Events.Details.Handler
{
    public class PlayerGetDetailsHandler : IRequestHandler<PlayerGetDetailsEvent, PlayerDetails>
    {
        readonly ILogger _logger;
        readonly IPlayerConnectionFactory _connectionFactory;
        public PlayerGetDetailsHandler(ILogger<PlayerGetDetailsHandler> logger, IPlayerConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task<PlayerDetails> Handle(PlayerGetDetailsEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("RequestId: " + request.Id);
            var connection = await _connectionFactory.GetConnection();
            var response = await connection.SendAction<PlayerDetails>("GetPlayer", request.Id);
            return response;
        }
    }
}