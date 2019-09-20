using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Plugin.Zone.System.Combat.Events.Client.Messsage;
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