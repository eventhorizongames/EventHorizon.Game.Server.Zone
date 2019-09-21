using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Client.Messsage
{
    public class ClientActionMessageFromCombatSystemEvent : ClientActionToAllEvent<MessageFromCombatSystemData>, INotification
    {
        public override string Action => "MessageFromCombatSystem";
        public override MessageFromCombatSystemData Data { get; set; }

    }
}