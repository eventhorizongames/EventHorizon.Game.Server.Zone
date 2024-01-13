namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Save;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using MediatR;

public struct SaveCombatSkillEvent : IRequest<SkillInstance>
{
    public SkillInstance Skill { get; set; }

    public SaveCombatSkillEvent(
        SkillInstance skill
    )
    {
        Skill = skill;
    }
}
