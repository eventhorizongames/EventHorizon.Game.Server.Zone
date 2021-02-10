namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Hasher.Create
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct CreateHashFromContentCommand
        : IRequest<CommandResult<string>>
    {
        public string Content { get; }

        public CreateHashFromContentCommand(
            string content
        )
        {
            Content = content;
        }
    }
}
