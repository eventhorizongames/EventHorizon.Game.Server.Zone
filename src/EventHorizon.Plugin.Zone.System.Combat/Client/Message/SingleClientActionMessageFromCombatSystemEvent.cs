using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Model.Client;
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