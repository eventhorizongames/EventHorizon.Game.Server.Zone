using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Gui
{
    public struct GuiLayout
    {
        public IEnumerable<GuiControlLayout> LayoutList { get; set; }
        public IEnumerable<GuiControlTemplate> TemplateList { get; set; }
        public IEnumerable<GuiControlData> DataList { get; set; }
    }
}