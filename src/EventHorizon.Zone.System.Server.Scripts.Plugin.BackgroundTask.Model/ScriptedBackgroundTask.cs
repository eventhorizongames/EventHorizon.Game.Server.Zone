namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model
{
    using EventHorizon.Zone.System.Server.Scripts.Model;

    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    public interface ScriptedBackgroundTask
        : ServerScript
    {
        /// <summary>
        /// A unique Id for this Scripted Background Task
        /// </summary>
        string TaskId { get; }
        /// <summary>
        /// The time in milliseconds this task should be triggered.
        /// </summary>
        int TaskPeriod { get; }
        /// <summary>
        /// A list of tags that can be used for identification or grouping of Tasks
        /// </summary>
        IEnumerable<string> TaskTags { get; }

        /// <summary>
        /// The method that should be triggered after the TaskPeriod on an interval.
        /// </summary>
        /// <param name="services">A set of services that can be used in the context of a Script.</param>
        /// <returns>A Task for this Triggered Background Task.</returns>
        Task TaskTrigger(
            ServerScriptServices services
        );
    }
}
