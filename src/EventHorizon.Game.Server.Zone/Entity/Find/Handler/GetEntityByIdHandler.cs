using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
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