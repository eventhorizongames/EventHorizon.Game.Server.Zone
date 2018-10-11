using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Gui
{
    public struct GuiControlLayout
    {
        public string Id { get; set; }
        public int Sort { get; set; }
        public List<GuiControlLayout> ControlList { get; set; }
    }
}