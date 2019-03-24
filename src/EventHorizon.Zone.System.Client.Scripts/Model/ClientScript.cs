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
            string fileName
        )
        {
            return new ClientScript(
                scriptsPath,
                path,
                fileName
            );
        }

        public string Name { get; }
        public string ScriptFileName { get; }
        public string ScriptPath { get; }
        public string ScriptString { get; }

        private ClientScript(
            string scriptsPath,
            string path,
            string fileName
        )
        {
            this.Name = fileName;
            this.ScriptFileName = fileName;
            this.ScriptPath = path;
            this.ScriptString = "";

            this.Name = this.GenerateName(
                path,
                fileName
            );
            this.ScriptString = this.LoadContent(
                scriptsPath
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

        public string LoadContent(
            string scriptsPath
        )
        {
            try
            {
                var fileName = Path.Combine(
                    scriptsPath,
                    this.ScriptPath,
                    this.ScriptFileName
                );
                using (var file = File.OpenText(fileName))
                {
                    return file.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {Name}",
                    ex
                );
            }
        }
    }
}