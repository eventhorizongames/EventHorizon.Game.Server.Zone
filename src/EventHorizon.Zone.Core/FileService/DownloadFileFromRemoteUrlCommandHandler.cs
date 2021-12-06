namespace EventHorizon.Zone.Core.FileService;

using System;
using System.IO;
using System.Net.Http;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;

using MediatR;

using Microsoft.Extensions.Logging;

public class DownloadFileFromRemoteUrlCommandHandler
    : IRequestHandler<DownloadFileFromRemoteUrlCommand, CommandResult<DownloadFileFromRemoteUrlResult>>
{
    private readonly HttpClient _httpClient;
    private readonly FileResolver _fileResolver;

    public DownloadFileFromRemoteUrlCommandHandler(
        HttpClient httpClient,
        FileResolver fileResolver
    )
    {
        _httpClient = httpClient;
        _fileResolver = fileResolver;
    }

    public async Task<CommandResult<DownloadFileFromRemoteUrlResult>> Handle(
        DownloadFileFromRemoteUrlCommand request,
        CancellationToken cancellationToken
    )
    {
        var url = request.Url;
        var fileFullName = request.FileFullName;
        var directoryFullName = Path.GetDirectoryName(
            request.FileFullName
        );

        if (directoryFullName.IsNullOrEmpty())
        {
            return "FILE_DIRECTORY_IS_NOT_VALID";
        }
        Directory.CreateDirectory(
            directoryFullName
        );

        using var response = await _httpClient.GetAsync(
            url,
            cancellationToken
        );
        using var stream = await response.Content.ReadAsStreamAsync(
            cancellationToken
        );
        using var zip = File.OpenWrite(
            fileFullName
        );
        stream.CopyTo(zip);

        var result = _fileResolver.GetFileInfo(
            fileFullName
        );

        return new DownloadFileFromRemoteUrlResult(
            result
        );
    }
}
