namespace EventHorizon.Zone.System.AssetServer.Tests.Base;

using AutoFixture.Xunit2;

using EventHorizon.Identity.AccessToken;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.AssetServer.Base;
using EventHorizon.Zone.System.AssetServer.Model;

using FluentAssertions;

using global::System;
using global::System.IO;
using global::System.Net;
using global::System.Net.Http;
using global::System.Text.Json;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;
using Moq.Protected;

using Xunit;


public class UploadFileToAssetServerCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReceiveSuccessfulContentResponseWhenContentIsValidJson(
        // Given
        string type,
        Uri uri,
        string fileFullName,
        string service,
        [Frozen] Mock<ILogger<UploadFileToAssetServerCommandHandler>> loggerMock,
        [Frozen] Mock<ISender> senderMock
    )
    {
        var result = new UploadAssetServerArtifactResult
        {
            Service = service,
            Path = "path",
        };
        var content = new MemoryStream();
        var delgatingHandlerMock = new Mock<DelegatingHandler>();
        delgatingHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        JsonSerializer.Serialize(
                            result
                        )
                    ),
                }
            );

        // When
        var handler = new UploadFileToAssetServerCommandHandler(
            loggerMock.Object,
            senderMock.Object,
            new HttpClient(delgatingHandlerMock.Object)
        );
        var actual = await handler.Handle(
            new UploadFileToAssetServerCommand(
                type,
                uri.ToString(),
                fileFullName,
                service,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData("{}")]
    [InlineAutoMoqData("{\"Service\": \"service\"}")]
    [InlineAutoMoqData("{\"Path\": \"path\"}")]
    public async Task ReceiveAPIErrorCodeResponseWhenStatusCodeIsSuccessfulAndContentIsNull(
        // Given
        string jsonContent,
        string type,
        Uri uri,
        string fileFullName,
        string service,
        [Frozen] Mock<ILogger<UploadFileToAssetServerCommandHandler>> loggerMock,
        [Frozen] Mock<ISender> senderMock
    )
    {
        var content = new MemoryStream();
        var delgatingHandlerMock = new Mock<DelegatingHandler>();
        delgatingHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonContent),
                }
            );

        // When
        var handler = new UploadFileToAssetServerCommandHandler(
            loggerMock.Object,
            senderMock.Object,
            new HttpClient(delgatingHandlerMock.Object)
        );
        var actual = await handler.Handle(
            new UploadFileToAssetServerCommand(
                type,
                uri.ToString(),
                fileFullName,
                service,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData(HttpStatusCode.Unauthorized)]
    [InlineAutoMoqData(HttpStatusCode.Forbidden)]
    public async Task ReceiveNotAuthorizedErrorCodeResponseWhenStatusCodeIsSuch(
        // Given
        HttpStatusCode statusCode,
        string type,
        Uri uri,
        string fileFullName,
        string service,
        [Frozen] Mock<ILogger<UploadFileToAssetServerCommandHandler>> loggerMock,
        [Frozen] Mock<ISender> senderMock
    )
    {
        var expected = "ASSET_SERVER_NOT_AUTHORIZED_API_ERROR";

        var content = new MemoryStream();
        var delgatingHandlerMock = new Mock<DelegatingHandler>();
        delgatingHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = statusCode,
                }
            );

        // When
        var handler = new UploadFileToAssetServerCommandHandler(
            loggerMock.Object,
            senderMock.Object,
            new HttpClient(delgatingHandlerMock.Object)
        );
        var actual = await handler.Handle(
            new UploadFileToAssetServerCommand(
                type,
                uri.ToString(),
                fileFullName,
                service,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReceiveTheErrorResultErrorCodeResponseWhenStatusCodeAndErrorResultAreNotSuccessful(
        // Given
        string errorCode,
        string type,
        Uri uri,
        string fileFullName,
        string service,
        [Frozen] Mock<ILogger<UploadFileToAssetServerCommandHandler>> loggerMock,
        [Frozen] Mock<ISender> senderMock
    )
    {
        var expected = errorCode;

        var content = new MemoryStream();
        var delgatingHandlerMock = new Mock<DelegatingHandler>();
        delgatingHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(
                        $"{{ \"ErrorCode\": \"{errorCode}\" }}"),
                }
            );

        // When
        var handler = new UploadFileToAssetServerCommandHandler(
            loggerMock.Object,
            senderMock.Object,
            new HttpClient(delgatingHandlerMock.Object)
        );
        var actual = await handler.Handle(
            new UploadFileToAssetServerCommand(
                type,
                uri.ToString(),
                fileFullName,
                service,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory]
    [InlineAutoMoqData("{}")]
    [InlineAutoMoqData("{\"ErrorCode\": null }")]
    [InlineAutoMoqData("{\"ErrorCode\": \"\" }")]
    public async Task ReceiveGenericErrorCodeResultResponseWhenErrorCodeInvalid(
        // Given
        string jsonString,
        string type,
        Uri uri,
        string fileFullName,
        string service,
        [Frozen] Mock<ILogger<UploadFileToAssetServerCommandHandler>> loggerMock,
        [Frozen] Mock<ISender> senderMock
    )
    {
        var expected = "ASSET_SERVER_API_ERROR";

        var content = new MemoryStream();
        var delgatingHandlerMock = new Mock<DelegatingHandler>();
        delgatingHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(jsonString),
                }
            );

        // When
        var handler = new UploadFileToAssetServerCommandHandler(
            loggerMock.Object,
            senderMock.Object,
            new HttpClient(delgatingHandlerMock.Object)
        );
        var actual = await handler.Handle(
            new UploadFileToAssetServerCommand(
                type,
                uri.ToString(),
                fileFullName,
                service,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReceiveGenericErrorCodeResultResponseWhenAnyExceptionIsThrown(
        // Given
        string type,
        Uri uri,
        string fileFullName,
        string service,
        [Frozen] Mock<ILogger<UploadFileToAssetServerCommandHandler>> loggerMock,
        [Frozen] Mock<ISender> senderMock
    )
    {
        var expected = "ASSET_SERVER_API_ERROR";

        var content = new MemoryStream();
        var delgatingHandlerMock = new Mock<DelegatingHandler>();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<RequestIdentityAccessTokenEvent>(),
                It.IsAny<CancellationToken>()
            )
        ).ThrowsAsync(
            new Exception("error")
        );

        // When
        var handler = new UploadFileToAssetServerCommandHandler(
            loggerMock.Object,
            senderMock.Object,
            new HttpClient(delgatingHandlerMock.Object)
        );
        var actual = await handler.Handle(
            new UploadFileToAssetServerCommand(
                type,
                uri.ToString(),
                fileFullName,
                service,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }
}
