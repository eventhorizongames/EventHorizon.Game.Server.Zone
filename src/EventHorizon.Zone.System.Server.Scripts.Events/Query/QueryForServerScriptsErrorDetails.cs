namespace EventHorizon.Zone.System.Server.Scripts.Model.Query;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;

using global::System.Collections.Generic;

using MediatR;

public struct QueryForServerScriptsErrorDetails
    : IRequest<CommandResult<ServerScriptsErrorDetailsResponse>>
{
}

public struct ServerScriptsErrorDetailsResponse
{
    public bool HasErrors { get; }
    public string ErrorCode { get; }
    public IEnumerable<GeneratedServerScriptErrorDetailsModel> ScriptErrorDetailsList { get; }

    public ServerScriptsErrorDetailsResponse(
        bool hasErrors,
        string errorCode,
        IEnumerable<GeneratedServerScriptErrorDetailsModel> scriptErrorDetailsList
    )
    {
        HasErrors = hasErrors;
        ErrorCode = errorCode;
        ScriptErrorDetailsList = scriptErrorDetailsList;
    }
}
