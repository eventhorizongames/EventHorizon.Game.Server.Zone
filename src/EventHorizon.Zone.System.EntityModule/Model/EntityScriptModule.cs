namespace EventHorizon.Zone.System.EntityModule.Model;

/// <summary>
/// These are instances of EntityScriptModule supplied by the server for the Engine/Client.
/// </summary>
public class EntityScriptModule
{
    public string Name { get; set; } = string.Empty;
    public string? InitializeScript { get; set; }
    public string? DisposeScript { get; set; }
    public string? UpdateScript { get; set; }
}
