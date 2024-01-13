namespace EventHorizon.Zone.System.Server.Scripts.Tests.Query;


using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Server.Scripts.Api;
using EventHorizon.Zone.System.Server.Scripts.Model.Query;
using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;
using EventHorizon.Zone.System.Server.Scripts.Query;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;


public class QueryForServerScriptsErrorDetailsHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldReturnErrorCodeAndDetailsWhenCommandIsHandled(
        // Given
        string errorCode,
        IEnumerable<GeneratedServerScriptErrorDetailsModel> scriptErrorDetailsList,
        [Frozen] Mock<ServerScriptsState> stateMock,
        QueryForServerScriptsErrorDetailsHandler handler
    )
    {
        var request = new QueryForServerScriptsErrorDetails();
        var expected = new ServerScriptsErrorDetailsResponse(
            true,
            errorCode,
            scriptErrorDetailsList
        );

        stateMock.Setup(
            mock => mock.ErrorCode
        ).Returns(
            errorCode
        );

        stateMock.Setup(
            mock => mock.ErrorDetailsList
        ).Returns(
            scriptErrorDetailsList
        );

        // When
        var actual = await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        actual.Result.Should().Be(
            expected
        );
    }
}
