namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct Skill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Target { get; set; }
        public long CoolDown { get; set; }
        public SkillEffect[] EffectList { get; set; }
    }
}