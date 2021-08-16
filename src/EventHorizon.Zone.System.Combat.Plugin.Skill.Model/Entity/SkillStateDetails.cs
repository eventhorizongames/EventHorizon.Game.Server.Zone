namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity
{
    using global::System;

    public struct SkillStateDetails
    {
        public string Id { get; set; }
        public DateTime CooldownFinishes { get; set; }
    }
}
