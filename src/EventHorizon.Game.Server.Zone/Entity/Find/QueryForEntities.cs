using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.State.Impl;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Find
{
    public struct QueryForEntities : IRequest<IEnumerable<IObjectEntity>>
    {
        public Func<IObjectEntity, bool> Query { get; set; }

        public struct QueryForEntitiesHandler : IRequestHandler<QueryForEntities, IEnumerable<IObjectEntity>>
        {
            readonly IEntityRepository _entityRepository;
            public QueryForEntitiesHandler(
                IEntityRepository entityRepository
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
}