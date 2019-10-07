using EventHorizon.Zone.Core.Client.Action;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Client.Messsage
{
    public class ClientActionMessageFromCombatSystemHandler
        : ClientActionToAllHandler<ClientActionMessageFromCombatSystemEvent, MessageFromCombatSystemData>,
            INotificationHandler<ClientActionMessageFromCombatSystemEvent>
    {
        public ClientActionMessageFromCombatSystemHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}