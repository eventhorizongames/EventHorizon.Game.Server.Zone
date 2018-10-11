using EventHorizon.Game.Server.Zone.Model.Gui;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Gui
{
    public struct RegisterControlTemplateEvent : INotification
    {
        public string Id { get; set; }
        public GuiControlTemplate Template { get; set; }
    }
}