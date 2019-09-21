using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Client.Messsage
{
    public class SingleClientActionMessageFromCombatSystemHandler
        :
        ClientActionToSingleHandler<SingleClientActionMessageFromCombatSystemEvent, MessageFromCombatSystemData>,
        INotificationHandler<SingleClientActionMessageFromCombatSystemEvent>
    {
        public SingleClientActionMessageFromCombatSystemHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}