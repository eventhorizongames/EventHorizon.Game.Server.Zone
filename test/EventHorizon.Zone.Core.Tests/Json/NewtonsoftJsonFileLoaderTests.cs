namespace EventHorizon.Zone.Core.Tests.Json
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Json;
    using EventHorizon.Zone.Core.Model.FileService;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;

    public class NewtonsoftJsonFileLoaderTests
    {
        [Fact]
        public async Task TestShouldReturnDefaultTypeWhenFileDoesNotExist()
        {
            // Given
            var fileFullName = "file-full-name";
            TestDataFile expected = null;

            var fileResolverMock = new Mock<FileResolver>();

            fileResolverMock.Setup(
                mock => mock.DoesFileExist(
                    fileFullName
                )
            ).Returns(
                false
            );

            // When
            var jsonFileLoader = new NewtonsoftJsonFileLoader(
                fileResolverMock.Object
            );
            var actual = await jsonFileLoader.GetFile<TestDataFile>(
                fileFullName
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public async Task TestShouldReturnDeserializeObjectWhenFileDoesExist()
        {
            // Given
            var fileFullName = "file-full-name";
            var fileText = @"{""TestProperty"":""test-property-value""}";
            var expected = "test-property-value";

            var fileResolverMock = new Mock<FileResolver>();

            fileResolverMock.Setup(
                mock => mock.DoesFileExist(
                    fileFullName
                )
            ).Returns(
                true
            );

            fileResolverMock.Setup(
                mock => mock.GetFileText(
                    fileFullName
                )
            ).Returns(
                fileText
            );

            // When
            var jsonFileLoader = new NewtonsoftJsonFileLoader(
                fileResolverMock.Object
            );
            var actual = await jsonFileLoader.GetFile<TestDataFile>(
                fileFullName
            );

            // Then
            actual.TestProperty.Should().Be(
                expected
            );
        }

        [Fact]
        public async Task ShouldDeserializeObjectPropertiesWithCaseInsensitivity()
        {
            // Given
            var fileFullName = "file-full-name";
            var fileText = @"{""testProperty"":""test-property-value""}";
            var expected = "test-property-value";

            var fileResolverMock = new Mock<FileResolver>();

            fileResolverMock.Setup(
                mock => mock.DoesFileExist(
                    fileFullName
                )
            ).Returns(
                true
            );

            fileResolverMock.Setup(
                mock => mock.GetFileText(
                    fileFullName
                )
            ).Returns(
                fileText
            );

            // When
            var jsonFileLoader = new NewtonsoftJsonFileLoader(
                fileResolverMock.Object
            );
            var actual = await jsonFileLoader.GetFile<TestDataFile>(
                fileFullName
            );

            // Then
            actual.TestProperty.Should().Be(
                expected
            );
        }

        public class TestDataFile
        {
            public string TestProperty { get; set; }
        }
    }
}
