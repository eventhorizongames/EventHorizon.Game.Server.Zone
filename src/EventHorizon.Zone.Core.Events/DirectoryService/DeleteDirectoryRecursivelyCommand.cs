namespace EventHorizon.Zone.Core.Events.DirectoryService;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record DeleteDirectoryRecursivelyCommand(
    string DirectoryFullName
) : IRequest<StandardCommandResult>;
