using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Find.Handler
{
    public class GetEntityByIdHandler : IRequestHandler<GetEntityByIdEvent, IObjectEntity>
    {
        readonly IEntityRepository _entityRepository;
        public GetEntityByIdHandler(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
        public async Task<IObjectEntity> Handle(GetEntityByIdEvent request, CancellationToken cancellationToken)
        {
            return await _entityRepository.FindById(request.EntityId);
        }
    }
}