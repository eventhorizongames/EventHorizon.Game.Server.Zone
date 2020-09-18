namespace EventHorizon.Zone.System.EntityModule.Model
{
    /// <summary>
    /// These are instances of EntityScriptModule supplied by the server for the Engine/Client.
    /// </summary>
    public struct EntityScriptModule
    {
        public string Name { get; set; }
        public string InitializeScript { get; set; }
        public string DisposeScript { get; set; }
        public string UpdateScript { get; set; }
        public string InitializeScriptName { get; set; }
        public string DisposeScriptName { get; set; }
        public string UpdateScriptName { get; set; }
    }
}