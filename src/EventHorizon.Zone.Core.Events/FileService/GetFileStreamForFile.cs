namespace EventHorizon.Zone.Core.Events.FileService;

using System.IO;

using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public record GetStreamForFileInfo(
    StandardFileInfo FileInfo
) : IRequest<Stream>;
