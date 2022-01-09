namespace EventHorizon.Zone.System.EntityModule.Reload;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.ClientActions;
using EventHorizon.Zone.System.EntityModule.Fetch;
using EventHorizon.Zone.System.EntityModule.Load;
using EventHorizon.Zone.System.EntityModule.Model.ClientActions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ReloadEntityModuleSystemCommandHandler
    : IRequestHandler<ReloadEntityModuleSystemCommand, StandardCommandResult>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly EntityModuleRepository _repository;

    public ReloadEntityModuleSystemCommandHandler(
        ISender sender,
        IPublisher publisher,
        EntityModuleRepository repository
    )
    {
        _sender = sender;
        _publisher = publisher;
        _repository = repository;
    }

    public async Task<StandardCommandResult> Handle(
        ReloadEntityModuleSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        _repository.Clear();

        await _sender.Send(
            new LoadEntityModuleSystemCommand(),
            cancellationToken
        );

        await _publisher.Publish(
            EntityModuleSystemReloadedClientActionToAllEvent.Create(
                new EntityModuleSystemReloadedClientActionData(
                    await _sender.Send(
                        new FetchBaseModuleListQuery(),
                        cancellationToken
                    ),
                    await _sender.Send(
                        new FetchPlayerModuleListQuery(),
                        cancellationToken
                    )
                )
            ),
            cancellationToken
        );

        return new StandardCommandResult();
    }

}
