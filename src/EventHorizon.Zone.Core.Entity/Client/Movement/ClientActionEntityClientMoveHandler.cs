using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using EventHorizon.Zone.Core.Client.Action;

namespace EventHorizon.Zone.Core.Entity.Client.Movement
{
    public class ClientActionEntityClientMoveHandler 
        : ClientActionToAllHandler<ClientActionEntityClientMoveToAllEvent, EntityClientMoveData>, 
            INotificationHandler<ClientActionEntityClientMoveToAllEvent>
    {
        public ClientActionEntityClientMoveHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}