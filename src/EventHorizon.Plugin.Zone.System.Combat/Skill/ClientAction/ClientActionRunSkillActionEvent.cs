using EventHorizon.Game.Server.Zone.Events.Client;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction
{
    public class ClientActionRunSkillActionEvent : ClientActionToAllEvent<ClientSkillActionEvent>, INotification
    {
        public override string Action => "RunSkillAction";

        public override ClientSkillActionEvent Data { get; set; }
    }
}