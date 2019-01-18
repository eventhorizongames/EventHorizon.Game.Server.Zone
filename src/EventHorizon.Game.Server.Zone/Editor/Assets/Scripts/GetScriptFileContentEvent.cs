using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts
{
    public struct GetScriptFileContentEvent : IRequest<EditorScriptFileContent>
    {
        public string Directory { get; set; }
        public string FileName { get; set; }
    }
}