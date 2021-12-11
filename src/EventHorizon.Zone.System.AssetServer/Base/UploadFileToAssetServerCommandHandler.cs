namespace EventHorizon.Zone.System.AssetServer.Base;

using EventHorizon.Identity.AccessToken;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.AssetServer.Model;

using global::System;
using global::System.Net;
using global::System.Net.Http;
using global::System.Net.Http.Headers;
using global::System.Net.Http.Json;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class UploadFileToAssetServerCommandHandler
    : IRequestHandler<UploadFileToAssetServerCommand, CommandResult<UploadAssetServerArtifactResult>>
{
    private readonly ILogger _logger;
    private readonly ISender _sender;
    private readonly HttpClient _httpClient;

    public UploadFileToAssetServerCommandHandler(
        ILogger<UploadFileToAssetServerCommandHandler> logger,
        ISender sender,
        HttpClient httpClient
    )
    {
        _logger = logger;
        _sender = sender;
        _httpClient = httpClient;
    }

    public async Task<CommandResult<UploadAssetServerArtifactResult>> Handle(
        UploadFileToAssetServerCommand request,
        CancellationToken cancellationToken
    )
    {
        var uploadType = request.Type;
        var url = request.Url;
        var fileFullName = request.FileFullName;
        var service = request.Service;
        var fileContent = request.Content;
        try
        {
            var accessToken = await _sender.Send(
                new RequestIdentityAccessTokenEvent(),
                cancellationToken
            );
            _httpClient.DefaultRequestHeaders.Add(
                "Authorization",
                $"Bearer {accessToken}"
            );

            using var form = new MultipartFormDataContent();
            using var fileContentAsStream = new StreamContent(
                fileContent
            );
            fileContentAsStream.Headers.ContentType = MediaTypeHeaderValue.Parse(
                "multipart/form-data"
            );
            form.Add(
                fileContentAsStream,
                "file",
                fileFullName
            );

            var response = await _httpClient.PostAsync(
                url,
                form,
                cancellationToken
            );
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode.HasFlag(
                    HttpStatusCode.Unauthorized
                ) || response.StatusCode.HasFlag(
                    HttpStatusCode.Forbidden
                ))
                {
                    _logger.LogError(
                        "Upload {UploadType} Artifact Not Authorized API Error. Service: {UploadService} | BackupFile: {UploadFileName}",
                        uploadType,
                        service,
                        fileFullName
                    );

                    return AssetServerErrorCodes.ASSET_SERVER_NOT_AUTHORIZED_API_ERROR;
                }

                var errorResult = await response.Content
                    .ReadFromJsonAsync<UploadAssetServerArtifactErrorResult>(
                        cancellationToken: cancellationToken
                    );
                var errorCode = (errorResult?.ErrorCode)
                    .IsNotNullOrEmpty()
                        ? errorResult.ErrorCode
                        : AssetServerErrorCodes.ASSET_SERVER_API_ERROR;

                _logger.LogError(
                    "Failed to Upload {UploadType} Artifact. Service: {UploadService} | UploadFile: {UploadFileName} | ErrorCode: {ErrorCode} | {@ErrorResult}",
                    uploadType,
                    service,
                    fileFullName,
                    errorCode,
                    errorResult
                );

                return errorCode;
            }

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UploadAssetServerArtifactResult>(
                cancellationToken: cancellationToken
            );
            if (result.IsNull()
                || result.Service.IsNullOrEmpty()
                || result.Path.IsNullOrEmpty()
            )
            {
                return AssetServerErrorCodes.ASSET_SERVER_API_ERROR;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to Upload {UploadType} Artifact. Service: {UploadService} | BackupFile: {UploadFileName}",
                uploadType,
                service,
                fileFullName
            );

            return AssetServerErrorCodes.ASSET_SERVER_API_ERROR;
        }
    }

    public class UploadAssetServerArtifactErrorResult
    {
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
    }
}
