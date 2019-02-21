using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile
{
    public struct GetCombatSkillsFileEvent : IRequest<CombatSkillsFile>
    {
        public string FileName { get; }

        public GetCombatSkillsFileEvent(
            string fileName
        ) {
            this.FileName = fileName;
        }
    }
}