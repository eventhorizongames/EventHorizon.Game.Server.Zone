namespace EventHorizon.Zone.System.Wizard.Json.Merge
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct MergeJsonStringsIntoSingleJsonStringCommand
        : IRequest<CommandResult<string>>
    {
        public string SourceJson { get; }
        public string UpdatedJson { get; }

        public MergeJsonStringsIntoSingleJsonStringCommand(
            string sourceJson,
            string updatedJson
        )
        {
            SourceJson = sourceJson;
            UpdatedJson = updatedJson;
        }
    }
}
