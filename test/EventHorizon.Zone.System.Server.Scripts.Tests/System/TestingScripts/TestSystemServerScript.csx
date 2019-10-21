using EventHorizon.Zone.System.Server.Scripts.System;

return new SystemServerScriptResponse(
    true,
    Data.Get<string>("ScriptMessage")
);