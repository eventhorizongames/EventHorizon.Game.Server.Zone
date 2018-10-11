using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Game.Server.Zone.Model.Gui.Options;
using EventHorizon.Game.Server.Zone.Model.Gui.Templates;

namespace EventHorizon.Plugin.Zone.System.Combat.Model.Gui
{
    public class CombatSystemGuiControlList
    {
        public GuiControlLayout Layout { get; set; }
        
        public GuiGridTemplate MainGrid { get; set; }
        public GuiPanelTemplate LifePanel { get; set; }
        public GuiLabelTemplate HealthLabel { get; set; }
        public GuiBarTemplate HealthBar { get; set; }
        public GuiLabelTemplate MagicLabel { get; set; }
        public GuiBarTemplate MagicBar { get; set; }
    }
}