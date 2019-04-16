using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Plugin.Zone.System.Combat.Events.Client.Messsage;
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