using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Gui
{
    public struct GuiTemplate
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public GuiGridLocation GridLocation { get; set; }
        public IDictionary<string, object> Options { get; set; }
    }
}