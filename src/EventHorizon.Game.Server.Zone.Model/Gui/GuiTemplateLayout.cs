using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Gui
{
    public interface GuiTemplateLayout
    {
        string Id { get; set; }
        List<GuiTemplateLayout> TemplateList { get; set; }
    }
}