namespace EventHorizon.Zone.System.Server.Scripts.Shared.Model
{
    using global::System;
    using global::System.Security.Cryptography;

    public class _ServerScriptModel
    {
        public string Id { get; }
        public string Hash { get; }

        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }

        public _ServerScriptModel(
            string fileName,
            string path,
            string scriptString
        )
        {
            Id = GenerateId(
                path,
                fileName
            );
            Hash = GenerateHash(
                scriptString
            );

            FileName = fileName;
            Path = path;
            ScriptString = scriptString;
        }

        private static string GenerateId(
            string path,
            string fileName
        )
        {
            var id = string.Join(
                "_",
                string.Join(
                    "_",
                    path.Split(
                        new char[] { global::System.IO.Path.DirectorySeparatorChar },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                ),
                fileName
            );
            if (id.StartsWith(
                "_"
            ))
            {
                return id[1..];
            }
            return id;
        }

        private static HashAlgorithm HashAlgorithm => MD5.Create();
        private static string GenerateHash(
            string content
        )
        {
            return Convert.ToBase64String(
                HashAlgorithm.ComputeHash(
                    content.ToBytes()
                )
            );
        }
    }
}
