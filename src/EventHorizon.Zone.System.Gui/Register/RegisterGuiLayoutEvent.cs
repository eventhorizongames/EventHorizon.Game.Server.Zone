using EventHorizon.Zone.System.Gui.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Gui
{
    public struct RegisterGuiLayoutCommand : IRequest
    {
        public GuiLayout Layout { get; set; }
    }
}