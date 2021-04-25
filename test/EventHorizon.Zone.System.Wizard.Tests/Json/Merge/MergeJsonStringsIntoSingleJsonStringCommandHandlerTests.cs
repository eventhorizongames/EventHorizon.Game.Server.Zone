namespace EventHorizon.Zone.System.Wizard.Tests.Json.Merge
{
    using EventHorizon.Zone.System.Wizard.Json.Merge;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Xunit;


    public class MergeJsonStringsIntoSingleJsonStringCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnErrorWhenSourceJsonIsNotObject()
        {
            // Given
            var sourceJson = "null";
            var updatedJson = "{}";
            var expected = "json_not_valid_type";

            // When
            var handler = new MergeJsonStringsIntoSingleJsonStringCommandHandler();
            var actual = await handler.Handle(
                new MergeJsonStringsIntoSingleJsonStringCommand(
                    sourceJson,
                    updatedJson
                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenNotValidJson()
        {
            // Given
            var sourceJson = "not-valid-json";
            var updatedJson = "not-valid-json";
            var expected = "json_not_valid_type";

            // When
            var handler = new MergeJsonStringsIntoSingleJsonStringCommandHandler();
            var actual = await handler.Handle(
                new MergeJsonStringsIntoSingleJsonStringCommand(
                    sourceJson,
                    updatedJson
                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }


        [Fact]
        public async Task ShouldReturnSourceJsonWhenSourceAndUpdateKindAreNotSame()
        {
            // Given
            var sourceJson = "{ \"type\": \"source\" }";
            var updatedJson = "\"is-a-string-type\"";
            var expected = "{ \"type\": \"source\" }";

            // When
            var handler = new MergeJsonStringsIntoSingleJsonStringCommandHandler();
            var actual = await handler.Handle(
                new MergeJsonStringsIntoSingleJsonStringCommand(
                    sourceJson,
                    updatedJson
                ),
                CancellationToken.None
            );


            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldIgnoreNullPropertiesWhenUpdateJsonIsNull()
        {
            // Given
            var sourceJson = "{ \"type\": \"source\", \"nullUpdateProperty\": \"keep-value\" }";
            var updatedJson = "{ \"type\": \"updated\", \"nullUpdateProperty\": null }";
            var expected = "{\"type\":\"updated\",\"nullUpdateProperty\":\"keep-value\"}";

            // When
            var handler = new MergeJsonStringsIntoSingleJsonStringCommandHandler();
            var actual = await handler.Handle(
                new MergeJsonStringsIntoSingleJsonStringCommand(
                    sourceJson,
                    updatedJson
                ),
                CancellationToken.None
            );


            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldMergeObjectsWhenSourceAndUpdatedKindMatchAndAreObject()
        {
            // Given
            var sourceJson = "{ \"type\": \"source\", \"object\": { \"sub-type\": \"source\" } }";
            var updatedJson = "{ \"type\": \"updated\", \"object\": { \"sub-type\": \"updated\" } }";
            var expected = "{\"type\":\"updated\",\"object\":{\"sub-type\":\"updated\"}}";

            // When
            var handler = new MergeJsonStringsIntoSingleJsonStringCommandHandler();
            var actual = await handler.Handle(
                new MergeJsonStringsIntoSingleJsonStringCommand(
                    sourceJson,
                    updatedJson
                ),
                CancellationToken.None
            );


            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldNotRemoveSourcePropertiesWhenNotInUpdatedJson()
        {
            // Given
            var sourceJson = "{ \"type\": \"source\", \"object\": { \"sub-type\": \"source\" }, \"number\": 123 }";
            var updatedJson = "{ \"type\": \"updated\", \"object\": { \"sub-type\": \"updated\" } }";
            var expected = "{\"type\":\"updated\",\"object\":{\"sub-type\":\"updated\"},\"number\":123}";

            // When
            var handler = new MergeJsonStringsIntoSingleJsonStringCommandHandler();
            var actual = await handler.Handle(
                new MergeJsonStringsIntoSingleJsonStringCommand(
                    sourceJson,
                    updatedJson
                ),
                CancellationToken.None
            );


            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().Be(expected);
        }


        [Fact]
        public async Task ShouldAddUpdatedPropertiesWhenNotInSourceJson()
        {
            // Given
            var sourceJson = "{ \"type\": \"source\", \"object\": { \"sub-type\": \"source\" }, \"number\": 123 }";
            var updatedJson = "{ \"type\": \"updated\", \"object\": { \"sub-type\": \"updated\" }, \"boolean\": false }";
            var expected = "{\"type\":\"updated\",\"object\":{\"sub-type\":\"updated\"},\"number\":123,\"boolean\":false}";

            // When
            var handler = new MergeJsonStringsIntoSingleJsonStringCommandHandler();
            var actual = await handler.Handle(
                new MergeJsonStringsIntoSingleJsonStringCommand(
                    sourceJson,
                    updatedJson
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
