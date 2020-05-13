namespace EventHorizon.Zone.System.ServerModule.Fetch
{
    using EventHorizon.Zone.System.ServerModule.Model;
    using EventHorizon.Zone.System.ServerModule.State;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class FetchServerModuleScriptListHandler 
        : IRequestHandler<FetchServerModuleScriptList, IEnumerable<ServerModuleScripts>>
    {
        private readonly ServerModuleRepository _serverModuleRepository;

        public FetchServerModuleScriptListHandler(
            ServerModuleRepository serverModuleRepository
        )
        {
            _serverModuleRepository = serverModuleRepository;
        }

        public Task<IEnumerable<ServerModuleScripts>> Handle(
            FetchServerModuleScriptList request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _serverModuleRepository.All()
            );
        }
    }
}