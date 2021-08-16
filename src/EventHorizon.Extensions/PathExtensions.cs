namespace System.IO
{
    public static class PathExtensions
    {
        public static string MakePathRelative(
            this string fromPath,
            string toPath
        ) => Path.GetRelativePath(
            GetPath(
                fromPath
            ),
            GetPath(
                toPath
            )
        );

        private static string GetPath(
            string path
        ) => path.StartsWith(Path.DirectorySeparatorChar)
            ? $".{path}"
            : $".{Path.DirectorySeparatorChar}{path}";
    }
}
