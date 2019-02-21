using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile
{
    public struct GetSystemCombatSkillScriptsFileEvent : IRequest<SystemCombatSkillScriptsFile>
    {
        public string FileName { get; }

        public GetSystemCombatSkillScriptsFileEvent(
            string fileName
        ) {
            this.FileName = fileName;
        }
    }
}