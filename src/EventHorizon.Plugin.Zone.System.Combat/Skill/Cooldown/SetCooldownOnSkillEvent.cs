using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Cooldown
{
    public struct SetCooldownOnSkillEvent : INotification
    {
        public long Caster { get; set; }
        public string SkillId { get; set; }
    }
}