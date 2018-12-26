using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.ServerModule.Model;
using EventHorizon.Zone.System.ServerModule.State;
using MediatR;

namespace EventHorizon.Zone.System.ServerModule.Fetch
{
    public class FetchServerModuleScriptListHandler : IRequestHandler<FetchServerModuleScriptListEvent, IEnumerable<ServerModuleScripts>>
    {
        readonly ServerModuleRepository _serverModuleRepository;
        public FetchServerModuleScriptListHandler(
            ServerModuleRepository serverModuleRepository
        )
        {
            _serverModuleRepository = serverModuleRepository;
        }
        public Task<IEnumerable<ServerModuleScripts>> Handle(FetchServerModuleScriptListEvent request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _serverModuleRepository.All()
            );
        }
    }
}