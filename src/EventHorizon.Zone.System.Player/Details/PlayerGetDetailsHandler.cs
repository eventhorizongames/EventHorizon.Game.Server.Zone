using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Events.Details;
using EventHorizon.Zone.System.Player.Model.Details;
using MediatR;

namespace EventHorizon.Zone.System.Player.Details
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