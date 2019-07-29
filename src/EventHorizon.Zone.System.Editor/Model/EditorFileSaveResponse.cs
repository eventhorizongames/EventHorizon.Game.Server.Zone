namespace EventHorizon.Zone.System.Editor.Model
{
    public struct EditorFileSaveResponse
    {
        public bool Successful { get; }
        public string ErrorCode { get; }
        public EditorFileSaveResponse(
            bool successful
        ) : this(successful, null)
        {
        }
        public EditorFileSaveResponse(
            bool successful,
            string errorCode
        )
        {
            Successful = successful;
            ErrorCode = errorCode;
        }
    }
}