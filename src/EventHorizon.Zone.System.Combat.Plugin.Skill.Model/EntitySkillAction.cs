using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Combat.Model
{
    public class EntitySkillAction : EntityAction
    {
        public static readonly EntitySkillAction ADD_SKILL = new EntitySkillAction("Skill.ADD_SKILL");

        protected EntitySkillAction(string type)
            : base(type)
        {
        }
    }
}
