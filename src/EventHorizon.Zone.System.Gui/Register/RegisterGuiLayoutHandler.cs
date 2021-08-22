namespace EventHorizon.Zone.System.Gui.Register
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Gui;
    using EventHorizon.Zone.System.Gui.Api;

    using MediatR;

    public class RegisterGuiLayoutHandler : IRequestHandler<RegisterGuiLayoutCommand>
    {
        readonly GuiState _guiState;
        public RegisterGuiLayoutHandler(
            GuiState guiState
        )
        {
            _guiState = guiState;
        }

        public Task<Unit> Handle(
            RegisterGuiLayoutCommand request,
            CancellationToken cancellationToken
        )
        {
            _guiState.AddLayout(
                request.Layout.Id,
                request.Layout
            );
            return Unit.Task;
        }
    }
}
