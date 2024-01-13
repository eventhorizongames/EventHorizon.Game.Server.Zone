namespace EventHorizon.Zone.System.Backup.Model;

public struct BackupFileResponse
{
    public bool Successful { get; }
    public BackupFileData FileData { get; }
    public string ErrorCode { get; }

    public BackupFileResponse(
        BackupFileData fileData
    ) : this(true, string.Empty)
    {
        FileData = fileData;
    }

    public BackupFileResponse(
        bool successful,
        string errorCode
    )
    {
        Successful = successful;
        FileData = default;
        ErrorCode = errorCode;
    }
}
