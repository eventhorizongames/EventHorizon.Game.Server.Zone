namespace EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Collections.Generic;
using global::System.IO;

public class ClientAsset
{
    private const string MetadataFileFullName = "metadata:fileFullName";

    public string Id { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public Dictionary<string, object> Data { get; set; } = new();

    public string GetFileFullName(
        string rootPath
    )
    {
        return Path.Combine(
            rootPath,
            "Assets",
            $"{Name}.{Id}.json"
        );
    }

    public void SetFileFullName(
        string fileFullName
    )
    {
        Data[MetadataFileFullName] = fileFullName;
    }

    public bool TryGetFileFullName(
        out string fileFullName
    )
    {
        fileFullName = string.Empty;
        if (Data.TryGetValue(
            MetadataFileFullName,
            out var fileFullNameData
        ) && fileFullNameData is string fileFullNameDataAsString)
        {
            // Save at fileFullName
            fileFullName = fileFullNameDataAsString;
            return true;
        }
        return false;
    }
}
