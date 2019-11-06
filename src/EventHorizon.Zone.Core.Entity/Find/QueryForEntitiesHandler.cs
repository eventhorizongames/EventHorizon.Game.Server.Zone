using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Find
{
    public class QueryForEntitiesHandler : IRequestHandler<QueryForEntities, IEnumerable<IObjectEntity>>
    {
        readonly EntityRepository _entityRepository;
        
        public QueryForEntitiesHandler(
            EntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }
        
        public Task<IEnumerable<IObjectEntity>> Handle(
            QueryForEntities request,
            CancellationToken cancellationToken
        )
        {
            return _entityRepository.Where(
                request.Query
            );
        }
    }
}