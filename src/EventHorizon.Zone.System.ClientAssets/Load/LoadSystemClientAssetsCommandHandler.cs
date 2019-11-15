using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
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

        public Task<Unit> Handle(
            LoadSystemClientAssetsCommand notification,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new LoadFileRecursivelyFromDirectory(
                 Path.Combine(
                    _serverInfo.ClientPath,
                    "Assets"
                ),
                OnProcessFile,
                null
            )
        );

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> _
        )
        {
            await _mediator.Publish(
                new AddClientAssetEvent
                {
                    ClientAsset = await _fileLoader.GetFile<ClientAsset>(
                        fileInfo.FullName
                    )
                }
            );
        }
    }
}