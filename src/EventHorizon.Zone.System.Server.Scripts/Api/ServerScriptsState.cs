namespace EventHorizon.Zone.System.Server.Scripts.Api;


using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;

using global::System.Collections.Generic;

public interface ServerScriptsState
{
    string CurrentHash { get; }
    string ErrorCode { get; }
    IEnumerable<GeneratedServerScriptErrorDetailsModel> ErrorDetailsList { get; }

    void UpdateHash(
        string hash
    );
    void SetErrorCode(
        string errorCode
    );
    void SetErrorState(
        IEnumerable<GeneratedServerScriptErrorDetailsModel> scriptErrorDetailsList
    );
}
