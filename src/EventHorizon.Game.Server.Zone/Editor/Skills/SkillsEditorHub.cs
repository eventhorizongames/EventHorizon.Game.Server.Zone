using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using EventHorizon.Game.Server.Zone.Editor.Model;
using EventHorizon.Game.Server.Zone.Editor.State.Get;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Save;
using EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Editor
{
    public partial class EditorHub : Hub
    {
        public Task<CombatSkillsFile> GetCombatSkills()
        {
            return _mediator.Send(new GetCombatSkillsFileEvent());
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