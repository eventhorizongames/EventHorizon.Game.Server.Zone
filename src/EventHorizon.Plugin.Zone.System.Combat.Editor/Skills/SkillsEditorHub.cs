using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Fetch;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Save;
using EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using EventHorizon.Plugin.Zone.System.Combat.Editor.Model;
using System;
using EventHorizon.Plugin.Zone.System.Combat.Editor.Skills.Query;

namespace EventHorizon.Plugin.Zone.System.Combat.Editor
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
        public Task SaveSystemCombatSkillScripts(SystemCombatSkillScriptsFile skillScriptFile)
        {
            // EventHorizon.Plugin.Zone.System.Combat.Editor.Skills.Query.EditorSystemCombatSkillScriptsFileQuery,EventHorizon.Plugin.Zone.System.Combat.Editor.Model.EditorSystemCombatSkillScriptsFile
            // await _mediator.Send(new SaveCombatSkillEvent
            // {
            //     Skill = skillInstance
            // });
            // await _mediator.Publish(new SaveCombatSkillsEvent());
            return Task.CompletedTask;
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
            await _mediator.Publish(new SaveCombatSkillsEvent());
        }
    }
}