namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public struct ServerScriptHashDetails
    {
        public bool IsDirty { get; }
        public string Hash { get; }

        public ServerScriptHashDetails(
            bool isDirty,
            string hash
        )
        {
            IsDirty = isDirty;
            Hash = hash;
        }
    }
}
