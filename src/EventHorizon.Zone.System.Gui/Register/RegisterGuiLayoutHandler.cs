using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Zone.System.Gui.Api;
using MediatR;

namespace EventHorizon.Zone.System.Gui.Register
{
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