namespace EventHorizon.Zone.Core.Events.FileService;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public record DownloadFileFromRemoteUrlCommand(
    string Url,
    string FileFullName
) : IRequest<CommandResult<DownloadFileFromRemoteUrlResult>>;

public record DownloadFileFromRemoteUrlResult(
    StandardFileInfo FileInfo
);
