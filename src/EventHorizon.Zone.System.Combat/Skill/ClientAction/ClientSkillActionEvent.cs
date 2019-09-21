using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Client;

namespace EventHorizon.Zone.System.Combat.Skill.ClientAction
{
    public struct ClientSkillActionEvent : IClientActionData
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }
}