namespace EventHorizon.Zone.System.Agent.Plugin.Companion.PopulateData;

using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class PopulatePlayerCompanionDataHandler
    : INotificationHandler<PopulateEntityDataEvent>
{
    public Task Handle(
        PopulateEntityDataEvent request,
        CancellationToken cancellationToken
    )
    {
        var entity = request.Entity;

        if (entity.Type != EntityType.PLAYER)
        {
            return Task.CompletedTask;
        }

        // Populate the Companion Management State on the Player.
        entity.PopulateData<CompanionManagementState>(
            CompanionManagementState.PROPERTY_NAME
        );

        return Task.CompletedTask;
    }
}
