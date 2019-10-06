using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Find
{
    public class GetEntityByIdHandler : IRequestHandler<GetEntityByIdEvent, IObjectEntity>
    {
        readonly EntityRepository _entityRepository;
        
        public GetEntityByIdHandler(
            EntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }
        
        public async Task<IObjectEntity> Handle(
            GetEntityByIdEvent request, 
            CancellationToken cancellationToken
        )
        {
            return await _entityRepository.FindById(
                request.EntityId
            );
        }
    }
}