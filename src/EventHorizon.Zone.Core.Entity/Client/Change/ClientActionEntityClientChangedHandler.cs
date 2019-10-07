using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using EventHorizon.Zone.Core.Client.Action;

namespace EventHorizon.Zone.Core.Entity.Client.Change
{
    public class ClientActionEntityClientChangedHandler
        : ClientActionToAllHandler<ClientActionEntityClientChangedToAllEvent, EntityChangedData>,
            INotificationHandler<ClientActionEntityClientChangedToAllEvent>
    {
        public ClientActionEntityClientChangedHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}