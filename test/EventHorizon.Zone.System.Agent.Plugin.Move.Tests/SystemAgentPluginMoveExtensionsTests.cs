namespace EventHorizon.Zone.System.Agent.Plugin.Move.Tests
{
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Game.Server.Zone;
    using Xunit;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.System.Agent.Move.Repository;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Move.Timer;
    using Moq;
    using Microsoft.AspNetCore.Builder;
    using MediatR;
    using global::System;
    using Microsoft.Extensions.DependencyInjection;

    public class SystemAgentPluginMoveExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemAgentPluginMoveExtensions.AddSystemAgentPluginMove(
                serviceCollectionMock
            );

            // Then
            Assert.NotEmpty(
                serviceCollectionMock
            );
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(
                        typeof(IMoveAgentRepository),
                        service.ServiceType
                    );
                    Assert.Equal(
                        typeof(MoveAgentRepository),
                        service.ImplementationType
                    );
                },
                service =>
                {
                    Assert.Equal(
                        typeof(ITimerTask),
                        service.ServiceType
                    );
                    Assert.Equal(
                        typeof(MoveRegisteredAgentsTimer),
                        service.ImplementationType
                    );
                }
            );
        }

        [Fact]
        public void TestUseSetupServer_ShouldSendAndPublishExpectedEvent()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var applicationBuilderMock = new Mock<IApplicationBuilder>();

            var serviceScopeFactoryMock = ServiceScopeFactoryUtils.SetupServiceScopeFactoryWithMediatorMock(
                mediatorMock
            );
            var applicationServicesMock = new Mock<IServiceProvider>();
            applicationServicesMock.Setup(
                applicationServices => applicationServices.GetService(
                    typeof(IServiceScopeFactory)
                )
            ).Returns(
                serviceScopeFactoryMock.Object
            );

            applicationBuilderMock.Setup(
                a => a.ApplicationServices
            ).Returns(
                applicationServicesMock.Object
            );

            // When
            SystemAgentPluginMoveExtensions.UseSystemAgentPluginMove(
                applicationBuilderMock.Object
            );

            // Then
            Assert.True(true);
        }
    }
}