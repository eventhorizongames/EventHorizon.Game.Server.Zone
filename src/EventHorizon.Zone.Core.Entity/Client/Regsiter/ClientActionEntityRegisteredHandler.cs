using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using EventHorizon.Zone.Core.Client.Action;

namespace EventHorizon.Zone.Core.Entity.Client.Register
{
    public class ClientActionEntityRegisteredHandler 
        : ClientActionToAllHandler<ClientActionEntityRegisteredToAllEvent, EntityRegisteredData>, 
            INotificationHandler<ClientActionEntityRegisteredToAllEvent>
    {
        public ClientActionEntityRegisteredHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}