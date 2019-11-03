using System;

namespace EventHorizon.Zone.System.Server.Scripts.Exceptions
{
    public class ServerScriptNotFound : Exception
    {
        public string ScriptId { get; }
        public ServerScriptNotFound(
            string scriptId,
            string message
        ) : base(
            message
        )
        {
            ScriptId = scriptId;
        }

    }
}