using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Skill.Fetch;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Skill.Save;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Model;
using System;
using EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Query;

namespace EventHorizon.Zone.System.Combat.Plugin.Editor.Skills
{
    [Authorize]
    public class SkillsEditorHub : Hub
    {
        readonly IMediator _mediator;
        public SkillsEditorHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            if (!Context.User.IsInRole("Admin"))
            {
                throw new Exception("no_role");
            }
            return Task.CompletedTask;
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
        public async Task SaveCombatSkill(SkillInstance skillInstance)
        {
            await _mediator.Send(new SaveCombatSkillEvent
            {
                Skill = skillInstance
            });
        }
    }
}