namespace EventHorizon.Zone.System.Agent.Tests;

using AutoFixture.Xunit2;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Test.Common.Utils;
using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Factory;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Save;
using EventHorizon.Zone.System.Agent.State;

using FluentAssertions;

using global::System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

using Moq;

using Xunit;

public class AgentExtensionsTests
{
    [Fact]
    public void TestAddAgent_ShouldConfigureServiceCollection()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();
        var configurationMock = new Mock<IConfiguration>();
        var config = new ConfigurationSection(
            new ConfigurationRoot(
                new List<IConfigurationProvider>()
            ),
            "Agent"
        );
        configurationMock.Setup(
            mock => mock.GetSection(
                "Agent"
            )
        ).Returns(
            config
        );

        // When
        serviceCollectionMock.AddSystemAgent(
            configurationMock.Object
        );

        // Then
        Assert.NotEmpty(
            serviceCollectionMock
        );
        Assert.Contains(
            serviceCollectionMock.Services,
            service =>
            {
                return typeof(IAgentRepository) == service.ServiceType
                    && typeof(AgentWrappedEntityRepository) == service.ImplementationType;
            }
        );
        Assert.Contains(
            serviceCollectionMock.Services,
            service =>
            {
                return typeof(ITimerTask) == service.ServiceType
                    && typeof(SaveAgentStateTimerTask) == service.ImplementationType;
            }
        );
        Assert.Contains(
            serviceCollectionMock.Services,
            service =>
            {
                return typeof(IAgentConnectionCache) == service.ServiceType
                    && typeof(AgentConnectionCache) == service.ImplementationType;
            }
        );
        Assert.Contains(
            serviceCollectionMock.Services,
            service =>
            {
                return typeof(IAgentConnectionFactory) == service.ServiceType
                    && typeof(AgentConnectionFactory) == service.ImplementationType;
            }
        );
    }

    [Theory, AutoMoqData]
    public void TestUseAgent_ShouldSendAndPublishExpectedEvent(
        // Given
        [Frozen] Mock<IApplicationBuilder> applicationBuilderMock
    )
    {
        // When
        var actual = applicationBuilderMock.Object
            .UseSystemAgent();

        // Then
        actual.Should().Be(
            applicationBuilderMock.Object
        );
    }
}
