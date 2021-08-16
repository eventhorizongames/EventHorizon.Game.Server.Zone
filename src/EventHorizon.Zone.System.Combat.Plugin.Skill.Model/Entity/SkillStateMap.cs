namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity
{
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Linq;

    public struct SkillStateMap
    {
        public IList<SkillStateDetails> List { get; set; }

        public SkillStateDetails Get(
            string skillId
        )
        {
            CheckForNull();
            var skill = List.Where(
                skillDetails => skillDetails.Id == skillId
            ).FirstOrDefault();
            if (skill.Id == null)
            {
                skill.Id = skillId;
            }
            return skill;
        }

        public bool Contains(
            string skillId
        )
        {
            CheckForNull();
            return List.Where(
                skill => skill.Id == skillId
            ).Count() > 0;
        }

        public SkillStateMap Set(
            SkillStateDetails skill
        )
        {
            CheckForNull();
            var newList = List.Where(
                listSkill => listSkill.Id != skill.Id
            ).ToList();
            newList.Add(
                skill
            );
            List = new ReadOnlyCollection<SkillStateDetails>(
                newList
            );

            return this;
        }

        private void CheckForNull()
        {
            if (List == null)
            {
                List = new ReadOnlyCollection<SkillStateDetails>(
                    new List<SkillStateDetails>()
                );
            }
        }
    }
}
