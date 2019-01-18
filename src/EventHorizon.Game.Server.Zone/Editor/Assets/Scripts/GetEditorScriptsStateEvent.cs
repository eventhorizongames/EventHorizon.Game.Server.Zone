using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts
{
    public struct GetEditorScriptsStateEvent : IRequest<EditorScriptsState>
    {

    }
}