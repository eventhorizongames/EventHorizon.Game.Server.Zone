using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client.Messsage
{
    public class ClientActionMessageFromCombatSystemHandler
        :
        ClientActionToAllHandler<ClientActionMessageFromCombatSystemEvent, MessageFromCombatSystemData>,
        INotificationHandler<ClientActionMessageFromCombatSystemEvent>
    {
        public ClientActionMessageFromCombatSystemHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}