namespace EventHorizon.Zone.System.Editor.Model;

public struct EditorResponse
{
    public bool Successful { get; }
    public string ErrorCode { get; }

    public EditorResponse(
        bool successful
    ) : this(successful, string.Empty)
    {
    }

    public EditorResponse(
        bool successful,
        string errorCode
    )
    {
        Successful = successful;
        ErrorCode = errorCode;
    }
}
