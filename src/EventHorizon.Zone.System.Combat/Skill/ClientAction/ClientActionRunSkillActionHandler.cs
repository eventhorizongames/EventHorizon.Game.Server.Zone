using EventHorizon.Zone.Core.Client.Action;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.ClientAction
{
    public class ClientActionRunSkillActionHandler 
        : ClientActionToAllHandler<ClientActionRunSkillActionEvent, ClientSkillActionEvent>, 
            INotificationHandler<ClientActionRunSkillActionEvent>
    {
        public ClientActionRunSkillActionHandler(
            IMediator mediator
        ) : base(mediator)
        { }
    }
}