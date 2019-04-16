using EventHorizon.Game.Server.Zone.Model.Gui;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Gui
{
    public struct RegisterGuiTemplateEvent : INotification
    {
        public GuiTemplate Template { get; set; }
    }
}