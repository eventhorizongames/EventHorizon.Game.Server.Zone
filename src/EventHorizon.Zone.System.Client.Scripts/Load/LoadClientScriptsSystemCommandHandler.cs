using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Zone.System.Client.Scripts.State;
using MediatR;

namespace EventHorizon.Zone.System.Client.Scripts.Load
{
    public class LoadClientScriptsSystemCommandHandler : INotificationHandler<LoadClientScriptsSystemCommand>
    {
        readonly ServerInfo _serverInfo;
        readonly ClientScriptRepository _clientScriptRepository;

        public LoadClientScriptsSystemCommandHandler(
            ServerInfo serverInfo,
            ClientScriptRepository clientScriptRepository
        )
        {
            _serverInfo = serverInfo;
            _clientScriptRepository = clientScriptRepository;
        }

        public Task Handle(
            LoadClientScriptsSystemCommand command,
            CancellationToken cancellationToken
        )
        {
            // Start Loading Script from Root Client Scripts Directory
            this.LoadFromDirectoryInfo(
                _serverInfo.ScriptsPath + Path.DirectorySeparatorChar,
                new DirectoryInfo(
                    GetClientScriptsPath()
                )
            );
            return Task.CompletedTask;
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
                // Create ClientScript AND Add to Repository
                _clientScriptRepository.Add(
                    ClientScript.Create(
                        scriptsPath,
                        MakeRelativePath(
                            scriptsPath,
                            fileInfo.DirectoryName
                        ),
                        fileInfo.Name
                    )
                );
            }
        }


        public static string MakeRelativePath(
            string fromPath,
            string toPath
        )
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private string GetClientScriptsPath()
        {
            return Path.Combine(
                _serverInfo.ScriptsPath,
                "Client"
            );
        }
    }
}