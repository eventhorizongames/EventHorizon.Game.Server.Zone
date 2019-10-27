using System;
using System.Collections.Generic;

namespace EventHorizon.Zone.System.Combat.Skill.Entity.State
{
    public struct SkillState
    {
        public static readonly string PROPERTY_NAME = "skillState";

        public SkillStateMap SkillMap { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "skillMap":
                    case "SkillMap":
                        return SkillMap;
                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "skillMap":
                    case "SkillMap":
                        SkillMap = (SkillStateMap)value;
                        break;
                    default:
                        break;
                }
            }
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