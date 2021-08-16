namespace EventHorizon.Zone.Core.Events.SubProcess
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.SubProcess;

    using MediatR;

    public struct StartSubProcessCommand
        : IRequest<CommandResult<SubProcessHandle>>
    {
        public string ApplicationFullName { get; }

        public StartSubProcessCommand(
            string applicationFullName
        )
        {
            ApplicationFullName = applicationFullName;
        }
    }
}
