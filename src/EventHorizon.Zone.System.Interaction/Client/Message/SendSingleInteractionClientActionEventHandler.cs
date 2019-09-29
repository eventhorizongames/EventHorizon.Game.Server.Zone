using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.System.Interaction.Events.Client;
using EventHorizon.Zone.System.Interaction.Model.Client;
using MediatR;

namespace EventHorizon.Zone.System.Interaction.Client.Messsage
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