namespace EventHorizon.Zone.System.Server.Scripts.Reload;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Server.Scripts.Complie;
using EventHorizon.Zone.System.Server.Scripts.Events.Reload;
using EventHorizon.Zone.System.Server.Scripts.Load;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ReloadServerScriptsSystemCommandHandler
    : IRequestHandler<ReloadServerScriptsSystemCommand, StandardCommandResult>
{
    private readonly IMediator _mediator;

    public ReloadServerScriptsSystemCommandHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<StandardCommandResult> Handle(
        ReloadServerScriptsSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(
            new LoadServerScriptsCommand(),
            cancellationToken
        );

        await _mediator.Send(
            new CompileServerScriptsFromSubProcessCommand(),
            cancellationToken
        );

        return new();
    }
}
