using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Register;
using MediatR;

namespace EventHorizon.Zone.System.ClientEntities.Load
{
    public class LoadSystemClientEntitiesCommandHandler : IRequestHandler<LoadSystemClientEntitiesCommand>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public LoadSystemClientEntitiesCommandHandler(
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
            LoadSystemClientEntitiesCommand request,
            CancellationToken cancellationToken
        )
        {
            // Register ClientEntity Instances from Path
            foreach (var clientEntityInstance in 
                await GetClientEntityInstancesFromPath(
                    _serverInfo.ClientEntityPath
                )
            )
            {
                // Register clientEntity Instance from File
                await _mediator.Publish(new RegisterClientEntityInstanceEvent
                {
                    ClientEntityInstance = clientEntityInstance
                });
            }
            return Unit.Value;
        }
        private async Task<IList<ClientEntityInstance>> GetClientEntityInstancesFromPath(
            string path
        )
        {
            // TODO: Load Recursively from root path
            var result = new List<ClientEntityInstance>();
            var directoryInfo = new DirectoryInfo(path);
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                result.Add(
                    await _fileLoader.GetFile<ClientEntityInstance>(
                        fileInfo.FullName
                    )
                );
            }
            return result;
        }
    }
}