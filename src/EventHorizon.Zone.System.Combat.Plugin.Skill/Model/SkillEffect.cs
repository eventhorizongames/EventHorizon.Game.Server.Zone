namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using global::System.Collections.Generic;

public struct SkillEffect
{
    public string Comment { get; set; }
    public string Effect { get; set; }
    public long Duration { get; set; }
    public Dictionary<string, object> Data { get; set; }
    public IList<SkillValidator> ValidatorList { get; set; }
    public IList<SkillEffect> Next { get; set; }
    public IList<SkillEffect> FailedList { get; set; }
}
