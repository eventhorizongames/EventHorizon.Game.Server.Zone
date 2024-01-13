namespace EventHorizon.Zone.Core.Entity.State;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Entity.Api;
using EventHorizon.Zone.Core.Entity.Model;
using EventHorizon.Zone.Core.Events.Json;
using EventHorizon.Zone.Core.Model.Entity;

using MediatR;

public class InMemoryEntitySettingsState
    : EntitySettingsState
{
    private int _currentConfigHash = -1;
    private int _currentDataHash = -1;
    private readonly IMediator _mediator;

    public ObjectEntityConfiguration EntityConfiguration
    {
        get;
        private set;
    } = new ObjectEntityConfigurationModel();

    public ObjectEntityData EntityData
    {
        get;
        private set;
    } = new ObjectEntityDataModel();

    public InMemoryEntitySettingsState(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<(bool Updated, ObjectEntityConfiguration OldConfig)> SetConfiguration(
        ObjectEntityConfiguration entityConfiguration,
        CancellationToken cancellationToken
    )
    {
        var (Valid, Hash) = await ValidateHash(
            _currentConfigHash,
            entityConfiguration,
            cancellationToken
        );
        if (!Valid)
        {
            return (false, EntityConfiguration);
        }

        _currentConfigHash = Hash;
        EntityConfiguration = entityConfiguration;

        return (true, EntityConfiguration);
    }

    public async Task<(bool Updated, ObjectEntityData OldData)> SetData(
        ObjectEntityData entityData,
        CancellationToken cancellationToken
    )
    {
        var (Valid, Hash) = await ValidateHash(
            _currentDataHash,
            entityData,
            cancellationToken
        );
        if (!Valid)
        {
            return (false, EntityData);
        }

        _currentDataHash = Hash;
        EntityData = entityData;

        return (true, EntityData);
    }

    private async Task<(bool Valid, int Hash)> ValidateHash(
        int currentHash,
        IDictionary<string, object> entityDictionary,
        CancellationToken cancellationToken
    )
    {
        var serailizeResult = await _mediator.Send(
            new SerializeToJsonCommand(
                entityDictionary
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
