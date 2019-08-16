namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillInstance
    {
        public static string CreateIdFromFileName(
            string fileName
        )
        {
            return fileName.Split(
                '.'
            )[0].ToLower();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SkillValidator[] ValidatorList { get; set; }
        public SkillEffect[] Next { get; set; }
        public SkillEffect[] FailedList { get; set; }

        public bool IsFound()
        {
            return !this.Equals(
                default(SkillInstance)
            );
        }
    }
}