namespace EventHorizon.Zone.System.Player.State
{
    using EventHorizon.Zone.Core.Events.Json;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Player.Api;
    using EventHorizon.Zone.System.Player.Model;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class InMemoryPlayerSettingsState
        : PlayerSettingsState
    {
        private int _currentHash = -1;
        private readonly IMediator _mediator;

        public ObjectEntityConfiguration PlayerConfiguration
        {
            get;
            private set;
        } = new PlayerObjectEntityConfigurationModel();

        public ObjectEntityData PlayerData
        {
            get;
            private set;
        } = new PlayerObjectEntityDataModel();

        public InMemoryPlayerSettingsState(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<(bool Updated, ObjectEntityConfiguration OldConfig)> SetConfiguration(
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

        public async Task<(bool Updated, ObjectEntityData OldData)> SetData(
            ObjectEntityData playerData,
            CancellationToken cancellationToken
        )
        {
            var serailizeResult = await _mediator.Send(
                new SerializeToJsonCommand(
                    playerData
                ),
                cancellationToken
            );
            if (!serailizeResult)
            {
                return (false, PlayerData);
            }

            var hash = serailizeResult
                .Result
                .Json.GetDeterministicHashCode();
            if (hash == _currentHash)
            {
                return (false, PlayerData);
            }

            _currentHash = hash;
            PlayerData = playerData;

            return (true, PlayerData);
        }
    }
}
