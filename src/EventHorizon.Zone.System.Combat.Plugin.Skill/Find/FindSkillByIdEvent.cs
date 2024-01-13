namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Find;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using MediatR;

public struct FindSkillByIdEvent : IRequest<SkillInstance>
{
    public string SkillId { get; set; }

    public FindSkillByIdEvent(
        string skillId
    )
    {
        SkillId = skillId;
    }
}
