using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Gui
{
    public struct GuiLayoutFile
    {
        public GuiControlLayout Layout { get; set; }
        public IList<GuiTemplate> TemplateList { get; set; }
    }
}