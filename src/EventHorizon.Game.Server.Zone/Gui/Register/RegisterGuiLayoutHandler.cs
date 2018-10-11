using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Game.Server.Zone.Gui.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Gui.Register
{
    public class RegisterGuiLayoutHandler : INotificationHandler<RegisterGuiLayoutEvent>
    {
        readonly GuiState _guiState;
        public RegisterGuiLayoutHandler(GuiState guiState)
        {
            _guiState = guiState;
        }
        public Task Handle(RegisterGuiLayoutEvent notification, CancellationToken cancellationToken)
        {
            _guiState.AddLayout(notification.Layout.Id, notification.Layout);
            return Task.CompletedTask;
        }
    }
}