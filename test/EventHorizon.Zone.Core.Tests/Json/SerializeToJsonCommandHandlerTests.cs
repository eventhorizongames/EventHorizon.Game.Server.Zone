namespace EventHorizon.Zone.Core.Tests.Json;

using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Api;
using EventHorizon.Zone.Core.Events.Json;
using EventHorizon.Zone.Core.Json;

using FluentAssertions;

using Moq;

using Xunit;

public class SerializeToJsonCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldCallIntoSerializeToJsonServiceWhenCommandIsHandled(
        // Given
        [Frozen] Mock<SerializeToJsonService> serializeToJsonServiceMock,
        object objectToSerialize,
        string expectedString,
        SerializeToJsonCommandHandler handler
    )
    {
        var expected = new SerializeToJsonResult(
            expectedString
        );
        serializeToJsonServiceMock.Setup(
            mock => mock.Serialize(
                objectToSerialize
            )
        ).Returns(expectedString);

        // When
        var actual = await handler.Handle(
            new SerializeToJsonCommand(
                objectToSerialize
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        actual.Result.Should().Be(expected);
    }
}
