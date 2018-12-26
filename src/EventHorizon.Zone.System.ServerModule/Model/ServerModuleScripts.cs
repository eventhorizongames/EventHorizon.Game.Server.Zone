namespace EventHorizon.Zone.System.ServerModule.Model
{
    /// <summary>
    /// These are instances of Scripts supplied by the server to the Engine/Client.
    /// </summary>
    public struct ServerModuleScripts
    {
        public string Name { get; set; }
        public string InitializeScriptString { get; set; }
        public string DisposeScriptString { get; set; }
        public string UpdateScriptString { get; set; }
    }
}