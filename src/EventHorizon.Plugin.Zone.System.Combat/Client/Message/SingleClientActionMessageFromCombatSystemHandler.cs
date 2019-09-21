using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client.Messsage
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