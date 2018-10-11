using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Game.Server.Zone.Model.Player;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Gui
{
    public struct FillGuiDataListForPlayerEvent : INotification
    {
        public PlayerEntity Player { get; set; }
        public IList<GuiControlData> DataListRef { get; set; }
    }
}