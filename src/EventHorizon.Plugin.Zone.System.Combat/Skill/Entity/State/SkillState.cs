using System;
using System.Collections.Generic;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Entity.State
{
    public struct SkillState
    {
        public static readonly string PROPERTY_NAME = "skillState";

        public IDictionary<string, EntitySkillState> SkillList { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "skillList":
                    case "SkillList":
                        return SkillList;
                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "skillList":
                    case "SkillList":
                        SkillList = (IDictionary<string, EntitySkillState>)value;
                        break;
                    default:
                        break;
                }
            }
        }

        public static readonly SkillState NEW = new SkillState
        {
            SkillList = new Dictionary<string, EntitySkillState>()
            {
                {
                    "fire_ball",
                    new EntitySkillState
                    {
                        CooldownFinishes = DateTime.Now
                    }
                },
                {
                    "move_to",
                    new EntitySkillState
                    {
                        CooldownFinishes = DateTime.Now
                    }
                },
                {
                    "capture_target",
                    new EntitySkillState
                    {
                        CooldownFinishes = DateTime.Now
                    }
                }
            }
        };
    }
}