using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Client;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction
{
    public struct ClientSkillActionEvent : IClientActionData
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }
}