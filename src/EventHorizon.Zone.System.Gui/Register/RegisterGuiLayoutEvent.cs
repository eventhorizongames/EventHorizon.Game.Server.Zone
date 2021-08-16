using EventHorizon.Zone.System.Gui.Model;

using MediatR;

namespace EventHorizon.Zone.Core.Events.Gui
{
    public struct RegisterGuiLayoutCommand : IRequest
    {
        public GuiLayout Layout { get; set; }
    }
}
