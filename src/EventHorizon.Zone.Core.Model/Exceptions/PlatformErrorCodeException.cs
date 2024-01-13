namespace EventHorizon.Zone.Core.Model.Exceptions;

using System;

[Serializable]
public class PlatformErrorCodeException
    : Exception
{
    public string ErrorCode { get; }

    public PlatformErrorCodeException(
        string errorCode,
        string message
    ) : base($"[{errorCode}] - {message}") {
        ErrorCode = errorCode;
    }
}
