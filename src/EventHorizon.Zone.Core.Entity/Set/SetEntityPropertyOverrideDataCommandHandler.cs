namespace EventHorizon.Zone.Core.Set;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Entity.Api;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.Filters;

using MediatR;

public class SetEntityPropertyOverrideDataCommandHandler
    : IRequestHandler<SetEntityPropertyOverrideDataCommand, CommandResult<IObjectEntity>>
{
    private readonly EntitySettingsCache _cache;

    public SetEntityPropertyOverrideDataCommandHandler(
        EntitySettingsCache cache
    )
    {
        _cache = cache;
    }

    public Task<CommandResult<IObjectEntity>> Handle(
        SetEntityPropertyOverrideDataCommand request,
        CancellationToken cancellationToken
    )
    {
        var forceSetList = _cache.EntityData.ForceSet;
        foreach (var property in _cache.EntityData.FilterOutForceSetKey())
        {
            if (!request.Entity.RawData.ContainsKey(
                property.Key
            ) || forceSetList.Contains(
                property.Key
            ))
            {
                request.Entity.RawData[property.Key] = property.Value;
            }
        }

        return new CommandResult<IObjectEntity>(
            request.Entity
        ).FromResult();
    }
}
