namespace EventHorizon.Zone.System.Server.Scripts.State
{
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;

    using global::System.Collections.Generic;

    public class StandardServerScriptsState
        : ServerScriptsState
    {
        public string CurrentHash { get; private set; } = string.Empty;
        public string ErrorCode { get; private set; } = string.Empty;
        public IEnumerable<GeneratedServerScriptErrorDetailsModel> ErrorDetailsList { get; private set; } = new List<GeneratedServerScriptErrorDetailsModel>();

        public void SetErrorCode(
            string errorCode
        )
        {
            CurrentHash = string.Empty;
            ErrorCode = errorCode;
        }

        public void SetErrorState(
            IEnumerable<GeneratedServerScriptErrorDetailsModel> scriptErrorDetailsList
        )
        {
            CurrentHash = string.Empty;
            ErrorDetailsList = scriptErrorDetailsList;
        }

        public void UpdateHash(
            string hash
        )
        {
            CurrentHash = hash;
            ErrorCode = string.Empty;
            ErrorDetailsList = new List<GeneratedServerScriptErrorDetailsModel>();
        }
    }
}
