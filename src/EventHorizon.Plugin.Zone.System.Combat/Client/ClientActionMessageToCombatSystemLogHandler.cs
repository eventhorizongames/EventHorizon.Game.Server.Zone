using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.External.Client;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client
{
    public class ClientActionMessageToCombatSystemLogHandler
        :
        ClientActionToAllHandler<ClientActionMessageToCombatSystemLogEvent, MessageToCombatSystemLogData>,
        INotificationHandler<ClientActionMessageToCombatSystemLogEvent>
    {
        public ClientActionMessageToCombatSystemLogHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}