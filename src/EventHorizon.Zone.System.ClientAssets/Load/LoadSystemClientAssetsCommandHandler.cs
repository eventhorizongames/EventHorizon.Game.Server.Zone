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
    public class LoadSystemClientAssetsCommandHandler : AsyncRequestHandler<LoadSystemClientAssetsCommand>
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
        protected override async Task Handle(
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
        }
        private string GetEntityAssetsPath()
        {
            return Path.Combine(
                _serverInfo.AssetsPath,
                "Client"
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