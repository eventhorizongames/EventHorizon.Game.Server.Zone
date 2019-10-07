using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using EventHorizon.Zone.Core.Client.Action;

namespace EventHorizon.Zone.Core.Entity.Client.Movement
{
    public class ClientActionEntityClientStoppingHandler 
        : ClientActionToAllHandler<ClientActionClientEntityStoppingToAllEvent, EntityClientStoppingData>, 
            INotificationHandler<ClientActionClientEntityStoppingToAllEvent>
    {
        public ClientActionEntityClientStoppingHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}