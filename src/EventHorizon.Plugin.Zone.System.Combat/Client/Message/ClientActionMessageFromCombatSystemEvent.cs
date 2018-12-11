using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client.Messsage
{
    public class ClientActionMessageFromCombatSystemEvent : ClientActionToAllEvent<MessageFromCombatSystemData>, INotification
    {
        public override string Action => "MessageFromCombatSystem";
        public override MessageFromCombatSystemData Data { get; set; }

    }
}