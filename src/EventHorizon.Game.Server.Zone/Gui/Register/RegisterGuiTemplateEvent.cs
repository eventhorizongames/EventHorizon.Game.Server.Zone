using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Events.Gui;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Gui.Register
{
    public class RegisterGuiTemplateHandler : INotificationHandler<RegisterGuiTemplateEvent>
    {
        readonly GuiState _guiState;
        public RegisterGuiTemplateHandler(
            GuiState guiState
        )
        {
            _guiState = guiState;
        }
        public Task Handle(
            RegisterGuiTemplateEvent notification,
            CancellationToken cancellationToken
        )
        {
            _guiState.Add(notification.Template.Id, notification.Template);
            return Task.CompletedTask;
        }
    }
}