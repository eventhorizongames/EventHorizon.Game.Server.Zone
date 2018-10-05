namespace EventHorizon.Game.Server.Zone.Plugin.State.Loader
{
    [System.Serializable]
    public class ToManyPlugins : System.Exception
    {
        public string FileName { get; }
        public string FilePath { get; }
        public ToManyPlugins(
            string message,
            string fileName,
            string filePath)
            : base(message)
        {
            FileName = fileName;
            FilePath = filePath;
        }
        protected ToManyPlugins(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(
                info,
                context)
        { }

    }
}