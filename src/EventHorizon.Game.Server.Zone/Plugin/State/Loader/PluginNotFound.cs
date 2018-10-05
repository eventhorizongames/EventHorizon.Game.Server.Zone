namespace EventHorizon.Game.Server.Zone.Plugin.State.Loader
{
    [System.Serializable]
    public class PluginNotFound : System.Exception
    {
        public string FileName { get; }
        public string FilePath { get; }
        public PluginNotFound(
            string message,
            string fileName,
            string filePath) 
            : base(
                message)
        {
            FileName = fileName;
            FilePath = filePath;
        }
        protected PluginNotFound(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(
                info, 
                context) { }

    }
}