namespace EventHorizon.Zone.System.Gui.Reload;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Gui.Api;
using EventHorizon.Zone.System.Gui.ClientActions;
using EventHorizon.Zone.System.Gui.Events.Layout;
using EventHorizon.Zone.System.Gui.Load;
using EventHorizon.Zone.System.Gui.Model.Client;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ReloadGuiSystemCommandHandler
    : IRequestHandler<ReloadGuiSystemCommand, StandardCommandResult>
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly GuiState _state;

    public ReloadGuiSystemCommandHandler(
        ISender sender,
        IPublisher publisher,
        GuiState state
    )
    {
        _sender = sender;
        _publisher = publisher;
        _state = state;
    }

    public async Task<StandardCommandResult> Handle(
        ReloadGuiSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        _state.Clear();

        await _sender.Send(
            new LoadSystemGuiCommand(),
            cancellationToken
        );

        await _publisher.Publish(
            GuiSystemReloadedClientActionToAllEvent.Create(
                new GuiSystemReloadedClientActionData(
                    await _sender.Send(
                        new GetGuiLayoutListForPlayerCommand(
                            default
                        ),
                        cancellationToken
                    )
                )
            ),
            cancellationToken
        );

        return new();
    }
}
