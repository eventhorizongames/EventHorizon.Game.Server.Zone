namespace EventHorizon.Zone.System.Server.Scripts.Model.Details
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Security.Cryptography;

    public struct ServerScriptDetails
    {
        private readonly static IEnumerable<string> EMPTY_TAG_LIST = new List<string>().AsReadOnly();

        public string Id { get; }
        public string Hash { get; }

        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }
        public IEnumerable<string> TagList { get; }

        public ServerScriptDetails(
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
            TagList = EMPTY_TAG_LIST;
        }

        public ServerScriptDetails(
            string id,
            string hash,
            string fileName,
            string path,
            string scriptString,
            IEnumerable<string> tagList
        )
        {
            Id = id;
            Hash = hash;

            FileName = fileName;
            Path = path;
            ScriptString = scriptString;
            TagList = tagList ?? EMPTY_TAG_LIST;
        }

        public static string GenerateId(
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
        public static string GenerateHash(
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
