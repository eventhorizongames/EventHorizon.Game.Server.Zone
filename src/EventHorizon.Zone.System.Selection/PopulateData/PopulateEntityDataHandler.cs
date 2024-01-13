namespace EventHorizon.Zone.System.Selection.PopulateData;

using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Selection.Model;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class PopulateEntityDataHandler
    : INotificationHandler<PopulateEntityDataEvent>
{
    public Task Handle(
        PopulateEntityDataEvent notification,
        CancellationToken cancellationToken
    )
    {
        var entity = notification.Entity;

        entity.PopulateData<SelectionState>(
            SelectionState.PROPERTY_NAME,
            SelectionState.NEW
        );

        return Task.CompletedTask;
    }
}
