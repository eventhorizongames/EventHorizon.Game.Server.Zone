using System;
using System.IO;

namespace EventHorizon.Zone.System.Client.Scripts.Model
{
    /// <summary>
    /// This is an instance of a Script supplied by the server to a Engine/Client.
    /// </summary>
    public struct ClientScript
    {
        public static ClientScript Create(
            string scriptsPath,
            string path,
            string fileName,
            string scriptString
        )
        {
            return new ClientScript(
                scriptsPath,
                path,
                fileName,
                scriptString
            );
        }

        public string Name { get; }
        public string ScriptFileName { get; }
        public string ScriptPath { get; }
        public string ScriptString { get; }

        private ClientScript(
            string scriptsPath,
            string path,
            string fileName,
            string scriptString
        )
        {
            this.Name = fileName;
            this.ScriptFileName = fileName;
            this.ScriptPath = path;
            this.ScriptString = scriptString;

            this.Name = this.GenerateName(
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