namespace EventHorizon.Zone.Core.Entity.Api;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.Entity;

public interface EntitySettingsState
    : EntitySettingsCache
{
    Task<(bool Updated, ObjectEntityConfiguration OldConfig)> SetConfiguration(
        ObjectEntityConfiguration entityConfiguration,
        CancellationToken cancellationToken
    );

    Task<(bool Updated, ObjectEntityData OldData)> SetData(
        ObjectEntityData entityData,
        CancellationToken cancellationToken
    );
}
