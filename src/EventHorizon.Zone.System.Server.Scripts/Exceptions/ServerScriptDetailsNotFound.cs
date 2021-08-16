namespace EventHorizon.Zone.System.Server.Scripts.Exceptions
{
    using global::System;

    public class ServerScriptDetailsNotFound : Exception
    {
        public string ScriptId { get; }

        public ServerScriptDetailsNotFound(
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
