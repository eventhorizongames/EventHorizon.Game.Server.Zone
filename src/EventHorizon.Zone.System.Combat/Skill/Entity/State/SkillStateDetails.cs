using System;

namespace EventHorizon.Zone.System.Combat.Skill.Entity.State
{
    public struct SkillStateDetails
    {
        public string Id { get; set; }
        public DateTime CooldownFinishes { get; set; }
    }
}