namespace EventHorizon.Zone.System.Gui.Model
{
    using global::System.Collections.Generic;

    public struct GuiLayoutControl
    {
        public string Id { get; set; }
        public int Sort { get; set; }
        public int Layer { get; set; }
        public string TemplateId { get; set; }
        public IDictionary<string, object> Options { get; set; }
        public GuiGridLocation GridLocation { get; set; }
        public IList<GuiLayoutControl> ControlList { get; set; }
    }
}
