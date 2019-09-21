using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.Plugin.Interaction.Model.Client;
using MediatR;

namespace EventHorizon.Zone.Plugin.Interaction.Events.Client
{
    public class SendSingleInteractionClientActionEvent : ClientActionToSingleEvent<InteractionClientActionData>, INotification
    {
        public override string ConnectionId { get; set; }
        public override string Action => "SERVER_INTERACTION_CLIENT_ACTION_EVENT";
        public override InteractionClientActionData Data { get; set; }

        public SendSingleInteractionClientActionEvent(
            string connectionId,
            InteractionClientActionData data
        )
        {
            ConnectionId = connectionId;
            Data = data;
        }
    }
}