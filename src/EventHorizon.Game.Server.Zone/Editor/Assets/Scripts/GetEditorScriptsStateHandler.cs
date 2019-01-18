using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using EventHorizon.Game.Server.Zone.External.Info;
using MediatR;
using IOPath = System.IO.Path;


namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts
{
    public struct GetEditorScriptsStateHandler : IRequestHandler<GetEditorScriptsStateEvent, EditorScriptsState>
    {
        readonly ServerInfo _serverInfo;
        public GetEditorScriptsStateHandler(
            ServerInfo serverInfo
        )
        {
            _serverInfo = serverInfo;
        }
        public async Task<EditorScriptsState> Handle(GetEditorScriptsStateEvent request, CancellationToken cancellationToken)
        {
            return new EditorScriptsState
            {
                // Load Actions Scripts File List
                Actions = GetEditorScriptFiles("Actions"),
                // Load Effects Scripts File List
                Effects = GetEditorScriptFiles("Effects"),
                // Load Routines Scripts File List
                Routines = GetEditorScriptFiles("Routines"),
                // Load Server Scripts File List
                Server = GetEditorScriptFiles("Server"),
                // Load Validators Scripts File List
                Validators = GetEditorScriptFiles("Validators"),
            };
        }

        private List<EditorScriptFile> GetEditorScriptFiles(string scriptDirectory)
        {
            var scriptsPath = _serverInfo.ScriptsPath;
            var scriptsFileNames = Directory.GetFiles(IOPath.Combine(scriptsPath, scriptDirectory));
            var scripts = new List<EditorScriptFile>(scriptsFileNames.Length);
            foreach (var script in scriptsFileNames)
            {
                scripts.Add(
                    new EditorScriptFile
                    {
                        FileDirectory = scriptDirectory,
                        FileName = new FileInfo(script).Name
                    }
                );
            }
            return scripts;
        }
    }
}