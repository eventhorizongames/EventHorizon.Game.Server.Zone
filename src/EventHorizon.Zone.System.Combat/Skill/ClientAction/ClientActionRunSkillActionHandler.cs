using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.ClientAction
{

    public class ClientActionRunSkillActionHandler : ClientActionToAllHandler<ClientActionRunSkillActionEvent, ClientSkillActionEvent>, INotificationHandler<ClientActionRunSkillActionEvent>
    {
        public ClientActionRunSkillActionHandler(
            IMediator mediator
        ) : base(mediator)
        { }
    }
}