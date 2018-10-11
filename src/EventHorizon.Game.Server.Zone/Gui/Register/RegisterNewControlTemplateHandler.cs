using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Game.Server.Zone.Gui.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Gui.Register
{
    public class RegisterNewControlTemplateHandler : INotificationHandler<RegisterControlTemplateEvent>
    {
        readonly IMediator _mediator;
        readonly GuiState _guiState;
        public RegisterNewControlTemplateHandler(IMediator mediator, GuiState guiState)
        {
            _guiState = guiState;
        }
        public Task Handle(RegisterControlTemplateEvent notification, CancellationToken cancellationToken)
        {
            _guiState.Add(notification.Id, notification.Template);
            return Task.CompletedTask;
        }
    }
}