namespace EventHorizon.Zone.Core.Entity.Update;

using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Entity.State;
using MediatR;

public class UpdateEntityCommandHandler : IRequestHandler<UpdateEntityCommand>
{
    private readonly EntityRepository _entityRepository;

    public UpdateEntityCommandHandler(EntityRepository entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task Handle(UpdateEntityCommand request, CancellationToken cancellationToken)
    {
        await _entityRepository.Update(request.Action, request.Entity);

    }
}
