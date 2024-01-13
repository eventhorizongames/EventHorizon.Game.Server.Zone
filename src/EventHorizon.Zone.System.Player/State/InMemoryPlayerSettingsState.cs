namespace EventHorizon.Zone.System.Player.State;

using EventHorizon.Zone.Core.Events.Json;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Player.Api;
using EventHorizon.Zone.System.Player.Model;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class InMemoryPlayerSettingsState
    : PlayerSettingsState
{
    private int _currentConfigHash = -1;
    private int _currentDataHash = -1;
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
        var (Valid, Hash) = await ValidateHash(
            _currentConfigHash,
            playerConfiguration,
            cancellationToken
        );
        if (!Valid)
        {
            return (false, PlayerConfiguration);
        }

        _currentConfigHash = Hash;
        PlayerConfiguration = playerConfiguration;

        return (true, PlayerConfiguration);
    }

    public async Task<(bool Updated, ObjectEntityData OldData)> SetData(
        ObjectEntityData playerData,
        CancellationToken cancellationToken
    )
    {
        var (Valid, Hash) = await ValidateHash(
            _currentDataHash,
            playerData,
            cancellationToken
        );
        if (!Valid)
        {
            return (false, PlayerData);
        }

        _currentDataHash = Hash;
        PlayerData = playerData;

        return (true, PlayerData);
    }

    private async Task<(bool Valid, int Hash)> ValidateHash(
        int currentHash,
        IDictionary<string, object> playerDictionary,
        CancellationToken cancellationToken
    )
    {
        var serailizeResult = await _mediator.Send(
            new SerializeToJsonCommand(
                playerDictionary
            ),
            cancellationToken
        );
        if (!serailizeResult)
        {
            return (false, -1);
        }

        var hash = serailizeResult
            .Result
            .Json.GetDeterministicHashCode();
        if (hash == currentHash)
        {
            return (false, -1);
        }

        return (true, hash);
    }
}
