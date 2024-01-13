namespace EventHorizon.Zone.Core.Model.Lifetime;

using System;

public struct OnServerStartupResult
{
    public bool Success { get; }
    public string ErrorCode { get; }

    public OnServerStartupResult(
        bool success,
        string errorCode = ""
    )
    {
        Success = success;
        ErrorCode = errorCode;
    }
}
