namespace EventHorizon.Zone.System.Player.State
{
    using EventHorizon.Zone.Core.Events.Json;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Player.Api;
    using EventHorizon.Zone.System.Player.Model;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class InMemoryPlayerConfigurationState
        : PlayerConfigurationState
    {
        private int _currentHash = -1;
        private readonly IMediator _mediator;

        public ObjectEntityConfiguration PlayerConfiguration
        {
            get;
            private set;
        } = new PlayerObjectEntityConfigurationModel();

        public InMemoryPlayerConfigurationState(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<(bool Updated, ObjectEntityConfiguration OldConfig)> Set(
            ObjectEntityConfiguration playerConfiguration,
            CancellationToken cancellationToken
        )
        {
            var serailizeResult = await _mediator.Send(
                new SerializeToJsonCommand(
                    playerConfiguration
                ),
                cancellationToken
            );
            if (!serailizeResult)
            {
                return (false, PlayerConfiguration);
            }

            var hash = serailizeResult
                .Result
                .Json.GetDeterministicHashCode();
            if (hash == _currentHash)
            {
                return (false, PlayerConfiguration);
            }

            _currentHash = hash;
            PlayerConfiguration = playerConfiguration;

            return (true, PlayerConfiguration);
        }
    }
}
