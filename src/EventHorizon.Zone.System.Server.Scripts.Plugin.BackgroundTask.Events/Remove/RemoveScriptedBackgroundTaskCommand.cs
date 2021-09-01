namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Remove
{
    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public struct RemoveScriptedBackgroundTaskCommand
        : IRequest<StandardCommandResult>
    {
        public string TaskId { get; }

        public RemoveScriptedBackgroundTaskCommand(
            string taskId
        )
        {
            TaskId = taskId;
        }
    }
}
