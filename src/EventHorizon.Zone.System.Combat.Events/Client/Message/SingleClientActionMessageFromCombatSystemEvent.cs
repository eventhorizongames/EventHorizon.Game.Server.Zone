using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client.Messsage
{
    public class SingleClientActionMessageFromCombatSystemEvent : ClientActionToSingleEvent<MessageFromCombatSystemData>, INotification
    {
        public override string ConnectionId { get; set; }
        public override string Action => "MessageFromCombatSystem";
        public override MessageFromCombatSystemData Data { get; set; }
    }
}