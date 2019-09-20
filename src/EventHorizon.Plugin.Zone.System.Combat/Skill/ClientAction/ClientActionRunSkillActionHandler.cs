using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction
{

    public class ClientActionRunSkillActionHandler : ClientActionToAllHandler<ClientActionRunSkillActionEvent, ClientSkillActionEvent>, INotificationHandler<ClientActionRunSkillActionEvent>
    {
        public ClientActionRunSkillActionHandler(
            IMediator mediator
        ) : base(mediator)
        { }
    }
}