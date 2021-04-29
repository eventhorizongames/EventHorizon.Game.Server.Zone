namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Tests.Create
{
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Create;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Xunit;

    public class CreateHashFromContentCommandHandlerTests
    {
        [Theory]
        [InlineData(null, "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
        [InlineData("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
        [InlineData("hash-me!", "7cd84255256b01b9da510860be120a9f17e2b99779f563937dd00021a0e98e7b")]
        public async Task ShouldGenerateExpectedHasBasedOnContent(
            // Given
            string content,
            string expected
        )
        {
            // When
            var handler = new CreateHashFromContentCommandHandler();
            var actual = await handler.Handle(
                new CreateHashFromContentCommand(
                    content
                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().Be(expected);
        }
    }
}
