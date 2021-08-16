using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.Core.ServerProperty.Fill;

using Microsoft.Extensions.Configuration;

using Moq;

using Xunit;

namespace EventHorizon.Zone.Core.Tests.ServerProperty.Fill
{
    public class FillHostServerPropertyHandlerTests
    {
        [Fact]
        public async Task TestShouldSetServerPropertyKeyOfHostWhenHandlingEvent()
        {
            // Given
            var expected = "host-property";

            var serverPropertyMock = new Mock<IServerProperty>();
            var configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(
                mock => mock["HOST"]
            ).Returns(
                expected
            );

            // When
            var handler = new FillHostServerPropertyHandler(
                serverPropertyMock.Object,
                configurationMock.Object
            );
            await handler.Handle(
                new FillServerPropertiesEvent(),
                CancellationToken.None
            );

            // Then
            serverPropertyMock.Verify(
                mock => mock.Set(
                    "HOST",
                    expected
                )
            );
        }
    }
}
