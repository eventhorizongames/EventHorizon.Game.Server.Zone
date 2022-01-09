namespace EventHorizon.Zone.System.EntityModule.Fetch;

using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Model;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class FetchPlayerModuleListQueryHandler
    : IRequestHandler<FetchPlayerModuleListQuery, IEnumerable<EntityScriptModule>>
{
    private readonly EntityModuleRepository _entityModuleRepository;

    public FetchPlayerModuleListQueryHandler(
        EntityModuleRepository entityModuleRepository
    )
    {
        _entityModuleRepository = entityModuleRepository;
    }

    public Task<IEnumerable<EntityScriptModule>> Handle(
        FetchPlayerModuleListQuery request,
        CancellationToken cancellationToken
    )
    {
        return _entityModuleRepository
            .ListOfAllPlayerModules()
            .FromResult();
    }
}
