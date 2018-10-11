using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Gui
{
    public interface GuiControlTemplate
    {
        string Id { get; set; }
        GuiControlType Type { get; set; }
        GuiGridLocation GridLocation { get; set; }
    }
}