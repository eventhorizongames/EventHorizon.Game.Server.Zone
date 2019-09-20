using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.Plugin.Interaction.Events.Client;
using EventHorizon.Zone.Plugin.Interaction.Model.Client;
using MediatR;

namespace EventHorizon.Zone.Plugin.Interaction.Client.Messsage
{
    public class SendSingleInteractionClientActionEventHandler :
        ClientActionToSingleHandler<SendSingleInteractionClientActionEvent, InteractionClientActionData>,
        INotificationHandler<SendSingleInteractionClientActionEvent>
    {
        public SendSingleInteractionClientActionEventHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}