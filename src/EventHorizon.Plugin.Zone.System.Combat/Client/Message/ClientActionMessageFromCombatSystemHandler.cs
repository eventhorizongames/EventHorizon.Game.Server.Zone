using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Plugin.Zone.System.Combat.Events.Client.Messsage;
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