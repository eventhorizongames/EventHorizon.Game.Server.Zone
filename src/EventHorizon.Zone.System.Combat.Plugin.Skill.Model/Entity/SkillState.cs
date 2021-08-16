
namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity
{
    using global::System;
    using global::System.Collections.Generic;

    public struct SkillState
    {
        public static readonly string PROPERTY_NAME = "skillState";

        public SkillStateMap SkillMap { get; set; }

        public SkillStateDetails GetSkill(
            string skillId
        ) => SkillMap.Get(skillId);

        public SkillState SetSkill(
            SkillStateDetails skill
        )
        {
            SkillMap = SkillMap.Set(
                skill
            );
            return this;
        }

        public static readonly SkillState NEW = new SkillState
        {
            SkillMap = new SkillStateMap
            {
                List = new List<SkillStateDetails>()
                {
                    new SkillStateDetails
                    {
                        Id = "Skills_FireBall.json",
                        CooldownFinishes = DateTime.Now
                    },
                    new SkillStateDetails
                    {
                        Id = "Skills_MoveTo.json",
                        CooldownFinishes = DateTime.Now
                    },
                    new SkillStateDetails
                    {
                        Id = "Skills_CaptureTarget.json",
                        CooldownFinishes = DateTime.Now
                    },
                }
            }
        };
    }
}
