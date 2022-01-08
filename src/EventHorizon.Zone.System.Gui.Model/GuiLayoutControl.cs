namespace EventHorizon.Zone.System.Gui.Model;

using global::System.Collections.Generic;

public class GuiLayoutControl
{
    public string Id { get; set; } = string.Empty;
    public int Sort { get; set; }
    public int Layer { get; set; }
    public string TemplateId { get; set; } = string.Empty;
    public IDictionary<string, object>? Options { get; set; }
    public GuiGridLocation? GridLocation { get; set; }
    public IList<GuiLayoutControl>? ControlList { get; set; }
}
