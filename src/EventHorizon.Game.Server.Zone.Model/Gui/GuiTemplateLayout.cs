using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Gui
{
    public struct GuiTemplateLayout
    {
        public string Id { get; set; }
        public List<GuiTemplateLayout> TemplateList { get; set; }
    }
}