using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Cooldown
{
    public struct SetCooldownOnSkillEvent : INotification
    {
        public long CasterId { get; set; }
        public string SkillId { get; set; }
        public long CoolDown { get; set; }
    }
}