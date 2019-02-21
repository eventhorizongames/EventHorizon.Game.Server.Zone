using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Load
{
    public struct LoadSystemCombatSkillScriptsEvent : INotification
    {
        public string FileName { get; }

        public LoadSystemCombatSkillScriptsEvent(
            string fileName
        ) {
            this.FileName = fileName;
        }
    }
}