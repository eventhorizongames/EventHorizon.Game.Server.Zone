namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillInstance
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SkillValidator[] ValidatorList { get; set; }
        public SkillEffect[] EffectList { get; set; }

        public bool IsFound()
        {
            return !this.Equals(
                default(SkillInstance)
            );
        }
    }
}