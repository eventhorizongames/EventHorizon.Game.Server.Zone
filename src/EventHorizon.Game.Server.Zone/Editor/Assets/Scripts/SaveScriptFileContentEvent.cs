using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts
{
    public struct SaveScriptFileContentEvent : INotification
    {
        public EditorScriptFileContent ScriptFileContent { get; set; }
    }
}