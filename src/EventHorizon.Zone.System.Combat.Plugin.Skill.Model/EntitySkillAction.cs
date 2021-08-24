namespace EventHorizon.Zone.System.Combat.Model
{
    using EventHorizon.Zone.Core.Model.Entity;

    public class EntitySkillAction
        : EntityAction
    {
        public static readonly EntitySkillAction ADD_SKILL = new("Skill.ADD_SKILL");

        protected EntitySkillAction(
            string type
        ) : base(type)
        {
        }
    }
}
