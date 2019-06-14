using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using EventHorizon.Game.Server.Zone.External.Info;
using MediatR;
using IOPath = System.IO.Path;
using EventHorizon.Game.Server.Zone.External.Extensions;

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
        public Task<EditorScriptsState> Handle(GetEditorScriptsStateEvent request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new EditorScriptsState
                {
                    // Load Actions Scripts File List
                    Client = GetEditorScriptFiles(
                        _serverInfo.ClientScriptsPath
                    ),
                    // Load Server Scripts File List
                    Server = GetEditorScriptFiles(
                        _serverInfo.ServerScriptsPath
                    ),
                }
            );
        }

        private List<EditorScriptFile> GetEditorScriptFiles(
            string scriptsDirectory
        )
        {
            var scripts = new List<EditorScriptFile>();
            LoadFromDirectory(
                _serverInfo.AppDataPath,
                $"{_serverInfo.AppDataPath}{IOPath.DirectorySeparatorChar}".MakePathRelative(
                    scriptsDirectory
                ),
                scripts
            );
            return scripts;
        }

        private static void LoadFromDirectory(
            string appDataPath,
            string scriptsDirectory,
            List<EditorScriptFile> scripts
        )
        {
            foreach (var scriptDirectory in
                Directory.GetDirectories(
                    IOPath.Combine(
                        appDataPath,
                        scriptsDirectory
                    )
                )
            )
            {
                LoadFromDirectory(
                    appDataPath,
                    $"{appDataPath}{IOPath.DirectorySeparatorChar}".MakePathRelative(
                        scriptDirectory
                    ),
                    scripts
                );
            }
            foreach (var script in
                Directory.GetFiles(
                    IOPath.Combine(
                        appDataPath,
                        scriptsDirectory
                    )
                )
            )
            {
                scripts.Add(
                    new EditorScriptFile
                    {
                        FileDirectory = scriptsDirectory.Split(
                            IOPath.DirectorySeparatorChar
                        ),
                        FileName = new FileInfo(script).Name
                    }
                );
            }
        }
    }
}