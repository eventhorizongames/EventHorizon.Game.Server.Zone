using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.External.Client;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client
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