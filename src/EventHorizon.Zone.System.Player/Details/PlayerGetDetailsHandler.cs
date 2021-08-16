namespace EventHorizon.Zone.System.Player.Details
{
    using EventHorizon.Zone.System.Player.Connection;
    using EventHorizon.Zone.System.Player.Events.Details;
    using EventHorizon.Zone.System.Player.Model.Details;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class PlayerGetDetailsHandler
        : IRequestHandler<PlayerGetDetailsEvent, PlayerDetails>
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
