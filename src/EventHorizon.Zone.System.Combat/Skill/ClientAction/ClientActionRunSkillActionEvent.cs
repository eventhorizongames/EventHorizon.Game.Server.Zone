using EventHorizon.Zone.Core.Events.Client;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.ClientAction
{
    public class ClientActionRunSkillActionEvent : ClientActionToAllEvent<ClientSkillActionEvent>, INotification
    {
        public override string Action => "RunSkillAction";

        public override ClientSkillActionEvent Data { get; set; }
    }
}