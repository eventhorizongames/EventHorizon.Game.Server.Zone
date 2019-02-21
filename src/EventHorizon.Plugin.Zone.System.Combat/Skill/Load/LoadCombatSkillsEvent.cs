using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Load
{
    public struct LoadCombatSkillsEvent : INotification
    {
        public string FileName { get; }

        public LoadCombatSkillsEvent(
            string fileName
        ) {
            this.FileName = fileName;
        }
    }
}