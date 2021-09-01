namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Register
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;

    using MediatR;

    public struct RegisterNewScriptedBackgroundTaskCommand
        : IRequest<StandardCommandResult>
    {
        public ScriptedBackgroundTask BackgroundTask { get; }

        public RegisterNewScriptedBackgroundTaskCommand(
            ScriptedBackgroundTask backgroundTask
        )
        {
            BackgroundTask = backgroundTask;
        }
    }
}
