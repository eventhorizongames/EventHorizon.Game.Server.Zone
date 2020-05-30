using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Query;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Fetch;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Save;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Editor
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