using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using EventHorizon.Game.Server.Zone.Editor.Model;
using EventHorizon.Game.Server.Zone.Editor.State.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Editor
{
    public partial class EditorHub : Hub
    {
        public Task<EditorScriptFileContent> GetScriptFileContent(string directory, string fileName)
        {
            return _mediator.Send(new GetScriptFileContentEvent
            {
                Directory = directory,
                FileName = fileName
            });
        }
        public Task SaveScriptFileContent(EditorScriptFileContent scriptFileContent)
        {
            return _mediator.Publish(new SaveScriptFileContentEvent
            {
                ScriptFileContent = scriptFileContent
            });
        }
    }
}