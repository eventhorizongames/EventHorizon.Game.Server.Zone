using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Model;
using EventHorizon.Zone.System.EntityModule.State;
using MediatR;

namespace EventHorizon.Zone.System.EntityModule.Fetch
{
    public class FetchBaseModuleListQueryHandler : IRequestHandler<FetchBaseModuleListQuery, IEnumerable<EntityScriptModule>>
    {
        readonly EntityModuleRepository _entityModuleRepository;
        public FetchBaseModuleListQueryHandler(
            EntityModuleRepository entityModuleRepository
        )
        {
            _entityModuleRepository = entityModuleRepository;
        }
        public Task<IEnumerable<EntityScriptModule>> Handle(FetchBaseModuleListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _entityModuleRepository.ListOfAllBaseModules()
            );
        }
    }
}