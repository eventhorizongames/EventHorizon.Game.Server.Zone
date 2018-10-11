namespace EventHorizon.Game.Server.Zone.Model.Gui.Options
{
    public class GuiBar : GuiControlOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public int TextSize { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
        public string BarColor { get; set; }
        public string BarDirection { get; set; }
        public string BorderColor { get; set; }
        public int Percent { get; set; }
        public int BorderThickness { get; set; }
    }
}