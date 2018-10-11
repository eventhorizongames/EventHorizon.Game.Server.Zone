using EventHorizon.Game.Server.Zone.Model.Gui.Options;

namespace EventHorizon.Game.Server.Zone.Model.Gui.Templates
{
    public struct GuiPanelTemplate : GuiControlTemplate
    {
        public string Id { get; set; }
        public GuiControlType Type { get; set; }
        public GuiGridLocation GridLocation { get; set; }
        public GuiPanel Options { get; set; }
    }
}