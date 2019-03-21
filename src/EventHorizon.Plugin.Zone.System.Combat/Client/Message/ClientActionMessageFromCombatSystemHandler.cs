using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.External.Client;
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