namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Editor;

using EventHorizon.Identity.Policies;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Query;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Fetch;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Save;

using global::System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize(UserIdOrClientIdOrAdminPolicy.PolicyName)]
public class SkillsEditorHub
    : Hub
{
    private readonly IMediator _mediator;

    public SkillsEditorHub(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public Task<EditorSystemCombatSkillScriptsFile> GetSystemCombatSkillScripts()
    {
        return _mediator.Send(
            new EditorSystemCombatSkillScriptsFileQuery()
        );
    }

    public async Task<EditorCombatSkills> GetCombatSkills()
    {
        return new EditorCombatSkills(
             await _mediator.Send(
                new GetAllCombatSkillsQuery()
            )
        );
    }

    public async Task SaveCombatSkill(
        SkillInstance skillInstance
    )
    {
        await _mediator.Send(
            new SaveCombatSkillEvent
            {
                Skill = skillInstance
            }
        );
    }
}
