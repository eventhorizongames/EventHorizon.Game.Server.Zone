using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client
{
    public class ClientActionMessageFromCombatSystemEvent : ClientActionToAllEvent<MessageFromCombatSystemData>, INotification
    {
        public override string Action => "MessageToCombatSystemLog";
        public override MessageFromCombatSystemData Data { get; set; }

    }
}