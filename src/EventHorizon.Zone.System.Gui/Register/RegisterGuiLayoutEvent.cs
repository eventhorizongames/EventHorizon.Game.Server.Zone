namespace EventHorizon.Zone.Core.Events.Gui
{
    using EventHorizon.Zone.System.Gui.Model;

    using MediatR;

    public struct RegisterGuiLayoutCommand : IRequest
    {
        public GuiLayout Layout { get; set; }
    }
}
