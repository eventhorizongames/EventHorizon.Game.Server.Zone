using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client
{
    public class SingleClientActionMessageFromCombatSystemEvent : ClientActionToSingleEvent<MessageFromCombatSystemData>, INotification
    {
        public override string ConnectionId { get; set; }
        public override string Action => "MessageToCombatSystemLog";
        public override MessageFromCombatSystemData Data { get; set; }
    }
}