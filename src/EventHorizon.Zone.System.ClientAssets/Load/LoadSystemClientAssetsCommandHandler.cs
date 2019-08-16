using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.ClientAssets.Add;
using EventHorizon.Zone.System.ClientAssets.Model;
using MediatR;

namespace EventHorizon.Zone.System.ClientAssets.Load
{
    public class LoadSystemClientAssetsCommandHandler : IRequestHandler<LoadSystemClientAssetsCommand>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public LoadSystemClientAssetsCommandHandler(
            IMediator mediator,
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        public async Task<Unit> Handle(
            LoadSystemClientAssetsCommand request,
            CancellationToken cancellationToken
        )
        {
            // Add ClientAssets from Path
            foreach (var clientAsset in await GetClientAssetsFromPath(GetEntityAssetsPath()))
            {
                // Add ClientAsset
                await _mediator.Publish(new AddClientAssetEvent
                {
                    ClientAsset = clientAsset
                });
            }
            return Unit.Value;
        }
        private string GetEntityAssetsPath()
        {
            return Path.Combine(
                _serverInfo.ClientPath,
                "Assets"
            );
        }
        private async Task<IList<ClientAsset>> GetClientAssetsFromPath(
            string path
        )
        {
            var result = new List<ClientAsset>();
            var directoryInfo = new DirectoryInfo(path);
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                result.Add(
                    await _fileLoader.GetFile<ClientAsset>(
                        fileInfo.FullName
                    )
                );
            }
            return result;
        }
    }
}