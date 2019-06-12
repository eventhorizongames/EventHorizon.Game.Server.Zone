using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Server.State;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.Server.Model;
using MediatR;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Server.Load
{
    public struct LoadServerScripts : IRequest
    {
        public struct LoadServerScriptsHandler : IRequestHandler<LoadServerScripts>
        {
            readonly ServerInfo _serverInfo;
            readonly ServerScriptRepository _serverScriptRepository;
            public LoadServerScriptsHandler(
                ServerInfo serverInfo,
                ServerScriptRepository serverScriptRepository
            )
            {
                _serverInfo = serverInfo;
                _serverScriptRepository = serverScriptRepository;
            }
            public Task<Unit> Handle(LoadServerScripts request, CancellationToken cancellationToken)
            {
                // Start Loading Script from Root Client Scripts Directory
                this.LoadFromDirectoryInfo(
                    GetClientScriptsPath() + IOPath.DirectorySeparatorChar,
                    new DirectoryInfo(
                        GetClientScriptsPath()
                    )
                );

                return Unit.Task;
            }

            private void LoadFromDirectoryInfo(
                string scriptsPath,
                DirectoryInfo directoryInfo
            )
            {
                // Load Scripts from Sub-Directories
                foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
                {
                    // Load Files From Directories
                    this.LoadFromDirectoryInfo(
                        scriptsPath,
                        subDirectoryInfo
                    );
                }
                // Load script files into Repository
                this.LoadFileIntoRepository(
                    scriptsPath,
                    directoryInfo
                );
            }


            private void LoadFileIntoRepository(
                string scriptsPath,
                DirectoryInfo directoryInfo
            )
            {
                foreach (var fileInfo in directoryInfo.GetFiles())
                {
                    using (var file = File.OpenText(fileInfo.FullName))
                    {
                        // Create ClientScript AND Add to Repository
                        _serverScriptRepository.Add(
                            ServerScript.Create(
                                new ServerScript.ServerScriptDetails
                                {
                                    FileName = fileInfo.Name,
                                    Path = MakeRelativePath(
                                        scriptsPath,
                                        fileInfo.DirectoryName
                                    ),
                                    ScriptString = file.ReadToEnd()
                                }
                            )
                        );
                    }
                }
            }

            public static string MakeRelativePath(
                string fromPath,
                string toPath
            )
            {
                if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
                if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

                var fromUri = new Uri(fromPath);
                var toUri = new Uri(toPath);

                if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

                var relativeUri = fromUri.MakeRelativeUri(
                    toUri
                );
                var relativePath = Uri.UnescapeDataString(
                    relativeUri.ToString()
                );

                if (toUri.Scheme.Equals(
                    "file",
                    StringComparison.InvariantCultureIgnoreCase
                ))
                {
                    relativePath = relativePath.Replace(
                        IOPath.AltDirectorySeparatorChar,
                        IOPath.DirectorySeparatorChar
                    );
                }

                return relativePath;
            }


            private string GetClientScriptsPath()
            {
                return IOPath.Combine(
                    _serverInfo.ServerPath,
                    "Scripts"
                );
            }
        }
    }
}