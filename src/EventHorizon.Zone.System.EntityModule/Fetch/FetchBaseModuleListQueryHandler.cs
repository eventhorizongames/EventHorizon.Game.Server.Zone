namespace EventHorizon.Zone.System.EntityModule.Fetch;

using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Model;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class FetchBaseModuleListQueryHandler
    : IRequestHandler<FetchBaseModuleListQuery, IEnumerable<EntityScriptModule>>
{
    private readonly EntityModuleRepository _entityModuleRepository;

    public FetchBaseModuleListQueryHandler(
        EntityModuleRepository entityModuleRepository
    )
    {
        _entityModuleRepository = entityModuleRepository;
    }

    public Task<IEnumerable<EntityScriptModule>> Handle(
        FetchBaseModuleListQuery request,
        CancellationToken cancellationToken
    )
    {
        return _entityModuleRepository
            .ListOfAllBaseModules()
            .FromResult();
    }
}
