namespace EventHorizon.Zone.System.Template.PopulateData;

using EventHorizon.Zone.Core.Events.Entity.Data;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class PopulateObjectEntityTemplateHandler
    : INotificationHandler<PopulateEntityDataEvent>
{
    public Task Handle(
        PopulateEntityDataEvent notification,
        CancellationToken cancellationToken
    )
    {
        //if (notification.Entity is PlayerEntity player)
        //{
        //    return Task.CompletedTask;
        //}

        //else if (notification.Entity is AgentEntity player)
        //{
        //    return Task.CompletedTask;
        //}

        //else if (notification.Entity is ClientEntity player)
        //{
        //    return Task.CompletedTask;
        //}

        return Task.CompletedTask;
    }
}
