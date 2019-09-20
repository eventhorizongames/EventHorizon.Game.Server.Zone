using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Plugin.Zone.System.Combat.Events.Client.Messsage;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client.Messsage
{
    public class ClientActionMessageFromCombatSystemEvent : ClientActionToAllEvent<MessageFromCombatSystemData>, INotification
    {
        public override string Action => "MessageFromCombatSystem";
        public override MessageFromCombatSystemData Data { get; set; }

    }
}