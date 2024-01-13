namespace EventHorizon.Zone.System.ClientEntities.Register;

using EventHorizon.Zone.System.ClientEntities.Client;
using EventHorizon.Zone.System.ClientEntities.Model.Client;
using EventHorizon.Zone.System.ClientEntities.PopulateData;
using EventHorizon.Zone.System.ClientEntities.State;
using EventHorizon.Zone.System.ClientEntities.Update;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class RegisterClientEntityCommandHandler : IRequestHandler<RegisterClientEntityCommand>
{
    private readonly IMediator _mediator;
    private readonly ClientEntityRepository _clientEntityRepository;

    public RegisterClientEntityCommandHandler(
        IMediator mediator,
        ClientEntityRepository entityRepository
    )
    {
        _mediator = mediator;
        _clientEntityRepository = entityRepository;
    }

    public async Task Handle(
        RegisterClientEntityCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = request.ClientEntity;
        await _mediator.Publish(new PopulateClientEntityDataEvent(entity), cancellationToken);
        _clientEntityRepository.Add(entity);
        await _mediator.Send(new SetClientEntityNodeDensity(entity), cancellationToken);

        await _mediator.Publish(
            SendClientEntityChangedClientActionToAllEvent.Create(
                new ClientEntityChangedClientActionData(entity)
            ),
            cancellationToken
        );
    }
}
