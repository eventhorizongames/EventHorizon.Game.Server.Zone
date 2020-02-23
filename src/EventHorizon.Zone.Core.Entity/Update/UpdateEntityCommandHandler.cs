using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Entity.State;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Update
{
    public class UpdateEntityCommandHandler : IRequestHandler<UpdateEntityCommand>
    {
        private readonly EntityRepository _entityRepository;

        public UpdateEntityCommandHandler(
            EntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }

        public async Task<Unit> Handle(
            UpdateEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            await _entityRepository.Update(
                request.Action,
                request.Entity
            );
            
            return Unit.Value;
        }
    }
}