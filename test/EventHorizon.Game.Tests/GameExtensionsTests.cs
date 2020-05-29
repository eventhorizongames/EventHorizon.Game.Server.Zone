namespace EventHorizon.Game.Tests
{
    using EventHorizon.Game.State;
    using EventHorizon.Game.Timer;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.TimerService;
    using FluentAssertions;
    using Xunit;

    public class GameExtensionsTests
    {
        [Fact]
        public void TestAddEntity_ShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            GameExtensions.AddGame(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(GameState), service.ServiceType);
                    Assert.Equal(typeof(InMemoryGameState), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ITimerTask), service.ServiceType);
                    Assert.Equal(typeof(RunPlayerListCaptureLogicTimerTask), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void ShouldReturnBuilderWhenCalledWithBuilder()
        {
            // Given
            var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = mocks.ApplicationBuilderMock.Object;

            // When
            var actual = GameExtensions.UseGame(
                mocks.ApplicationBuilderMock.Object
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
