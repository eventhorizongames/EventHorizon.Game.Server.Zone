using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Gui;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Gui
{
    public struct GenerateGuiControlDataEvent : INotification
    {
        // TODO: Look at moveing this to a an abstract builder service.
        public List<GuiControlOptions> GuiControlOptionsList { get; set; }
    }
}