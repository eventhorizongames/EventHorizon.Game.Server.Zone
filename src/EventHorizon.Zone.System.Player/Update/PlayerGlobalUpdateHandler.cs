namespace EventHorizon.Zone.System.Player.Update
{
    using EventHorizon.Zone.System.Player.Connection;
    using EventHorizon.Zone.System.Player.Events.Update;
    using EventHorizon.Zone.System.Player.Mapper;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class PlayerGlobalUpdateHandler 
        : INotificationHandler<PlayerGlobalUpdateEvent>
    {
        private readonly PlayerServerConnectionFactory _connectionFactory;
        
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