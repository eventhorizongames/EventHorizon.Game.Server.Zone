namespace EventHorizon.Zone.System.Client.Scripts.Model
{
    using global::System.IO;

    /// <summary>
    /// This is an instance of a Script supplied by the server to the Engine/Client.
    /// </summary>
    public struct ClientScript
    {
        public static ClientScript Create(
            ClientScriptType scriptType,
            string path,
            string fileName,
            string scriptString
        )
        {
            return new ClientScript(
                scriptType,
                path,
                fileName,
                scriptString
            );
        }

        public string Name { get; }
        public ClientScriptType ScriptType { get; }
        public string ScriptFileName { get; }
        public string ScriptPath { get; }
        public string ScriptString { get; }

        private ClientScript(
            ClientScriptType scriptType,
            string path,
            string fileName,
            string scriptString
        )
        {
            Name = fileName;
            ScriptType = scriptType;
            ScriptFileName = fileName;
            ScriptPath = path;
            ScriptString = scriptString;

            Name = GenerateName(
                path,
                fileName
            );
        }

        private string GenerateName(
            string path,
            string fileName
        )
        {
            return string.Join(
                "_",
                string.Join(
                    "_",
                    path.Split(
                        Path.DirectorySeparatorChar
                    )
                ),
                fileName
            );
        }
    }
}
