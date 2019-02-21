using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Gui
{
    public struct LoadCombatSystemGuiEvent : INotification
    {
        public string FileName { get; }

        public LoadCombatSystemGuiEvent(
            string fileName
        ) {
            this.FileName = fileName;
        }
    }
}