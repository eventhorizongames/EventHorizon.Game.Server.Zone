namespace EventHorizon.Game.Server.Zone.Model.Gui.Options
{
    public class GuiGrid : GuiControlOptions
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public string BackgroundColor { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingLeft { get; set; }
        public int PaddingRight { get; set; }
    }
}