namespace EventHorizon.Zone.System.Player.Tests.Model.Client
{
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.Player.Model.Client;
    using EventHorizon.Zone.System.Player.Tests.TestingModels;

    using FluentAssertions;

    using Xunit;

    public class PlayerSystemReloadedEventDataTests
    {
        [Theory, AutoMoqData]
        public void ShouldReturnConfigWhenCreated(
            // Given
            ObjectEntityConfigurationTestModel playerConfiguration
        )
        {
            // When
            var model = new PlayerSystemReloadedEventData(
                playerConfiguration
            );

            // Then
            model.PlayerConfiguration.Should().BeEquivalentTo(
                playerConfiguration
            );
        }
    }
}
