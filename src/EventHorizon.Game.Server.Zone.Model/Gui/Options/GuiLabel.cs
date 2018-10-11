namespace EventHorizon.Game.Server.Zone.Model.Gui.Options
{
    public class GuiLabel : GuiControlOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public int TextSize { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
        public int Alignment { get; set; }
        public int BorderThickness { get; set; }
    }
}