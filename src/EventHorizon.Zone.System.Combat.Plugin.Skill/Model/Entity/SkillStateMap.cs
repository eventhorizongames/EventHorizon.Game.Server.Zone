using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity
{
    public struct SkillStateMap
    {
        public IList<SkillStateDetails> List { get; set; }

        public SkillStateDetails Get(
            string skillId
        )
        {
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
            return List.Where(
                skill => skill.Id == skillId
            ).Count() > 0;
        }

        public void Set(
            SkillStateDetails skill
        )
        {
            var newList = List.Where(
                listSkill => listSkill.Id != skill.Id
            ).ToList();
            newList.Add(
                skill
            );
            List = new ReadOnlyCollection<SkillStateDetails>(
                newList
            );
        }
    }
}