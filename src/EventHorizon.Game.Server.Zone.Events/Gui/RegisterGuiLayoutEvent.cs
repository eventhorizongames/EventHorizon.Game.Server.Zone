using EventHorizon.Game.Server.Zone.Model.Gui;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Gui
{
    public struct RegisterGuiLayoutEvent : INotification
    {
        public GuiControlLayout Layout { get; set; }
    }
}