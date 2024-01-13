namespace EventHorizon.Zone.System.ServerModule.Reload;

using EventHorizon.Zone.System.ServerModule.Actions.Reload;
using EventHorizon.Zone.System.ServerModule.Fetch;
using EventHorizon.Zone.System.ServerModule.Load;
using EventHorizon.Zone.System.ServerModule.Model.Client;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class ReloadServerModuleSystemHandler : IRequestHandler<ReloadServerModuleSystem>
{
    private readonly IMediator _mediator;

    public ReloadServerModuleSystemHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(
        ReloadServerModuleSystem request,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Publish(new LoadServerModuleSystem());

        await _mediator.Publish(
            SendServerModuleSystemReloadedClientActionToAllEvent.Create(
                new ServerModuleSystemReloadedClientActionData(
                    await _mediator.Send(new FetchServerModuleScriptList())
                )
            )
        );
    }
}
