namespace EventHorizon.Zone.System.Gui.Register;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Gui.Api;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class RegisterGuiLayoutCommandHandler
    : IRequestHandler<RegisterGuiLayoutCommand, StandardCommandResult>
{
    private readonly GuiState _guiState;

    public RegisterGuiLayoutCommandHandler(
        GuiState guiState
    )
    {
        _guiState = guiState;
    }

    public Task<StandardCommandResult> Handle(
        RegisterGuiLayoutCommand request,
        CancellationToken cancellationToken
    )
    {
        _guiState.AddLayout(
            request.Layout.Id,
            request.Layout
        );
        return new StandardCommandResult()
            .FromResult();
    }
}
