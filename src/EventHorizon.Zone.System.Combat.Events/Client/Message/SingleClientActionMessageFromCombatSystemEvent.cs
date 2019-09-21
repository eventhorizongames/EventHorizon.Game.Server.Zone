using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Events.Client.Messsage
{
    // TODO: Move this functionality into a Messaging System for platform sent Message 
    public class SingleClientActionMessageFromCombatSystemEvent : ClientActionToSingleEvent<MessageFromCombatSystemData>, INotification
    {
        public override string ConnectionId { get; set; }
        public override string Action => "MessageFromCombatSystem";
        public override MessageFromCombatSystemData Data { get; set; }
    }
}