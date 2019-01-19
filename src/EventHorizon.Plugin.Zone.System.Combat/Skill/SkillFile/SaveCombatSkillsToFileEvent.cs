using System.Collections.Generic;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile
{
    public struct SaveCombatSkillsToFileEvent : INotification
    {
        public IList<SkillInstance> SkillList { get; set; }
    }
}