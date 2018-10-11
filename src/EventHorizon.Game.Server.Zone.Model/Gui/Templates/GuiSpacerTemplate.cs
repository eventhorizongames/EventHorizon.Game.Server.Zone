using EventHorizon.Game.Server.Zone.Model.Gui.Options;

namespace EventHorizon.Game.Server.Zone.Model.Gui.Templates
{
    public struct GuiSpacerTemplate : GuiControlTemplate
    {
        public string Id { get; set; }
        public GuiControlType Type { get; set; }
        public GuiGridLocation GridLocation { get; set; }
        public GuiSpacer Options { get; set; }
    }
}