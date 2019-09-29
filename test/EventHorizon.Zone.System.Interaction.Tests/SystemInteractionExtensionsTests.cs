using System;
using System.Threading;
using EventHorizon.Game.Server.Zone;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.System.Interaction.Script.Load;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EventHorizon.Zone.System.Interaction.Tests.Agent.Behavior
{
public class SystemInteractionExtensionsTests : TestFixtureBase
    {
        public SystemInteractionExtensionsTests(
            ITestOutputHelper testOutputHelper
        ) : base(
            testOutputHelper
        )
        {
        }
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemInteractionExtensions.AddSystemInteraction(
                serviceCollectionMock
            );

            // Then
            Assert.Empty(
                serviceCollectionMock
            );
        }

        [Fact]
        public void TestUseSetupServer_ShouldSendAndPublishExpectedEvent()
        {
            // Given
            var expectedCommand = new LoadInteractionScriptsCommand();

            var mediatorMock = new Mock<IMediator>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(
                serviceProvider => serviceProvider
                    .GetService(
                        typeof(IMediator)
                    )
            ).Returns(
                mediatorMock.Object
            );
            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(
                serviceScopeFactory => serviceScopeFactory.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );
            var applicationServicesMock = new Mock<IServiceProvider>();
            applicationServicesMock.Setup(
                applicationServices => applicationServices.GetService(
                    typeof(IServiceScopeFactory)
                )
            ).Returns(
                serviceScopeFactoryMock.Object
            );

            var applicationBuilderMock = new Mock<IApplicationBuilder>();
            applicationBuilderMock.Setup(
                a => a.ApplicationServices
            ).Returns(
                applicationServicesMock.Object
            );

            // When
            SystemInteractionExtensions.UseSystemInteraction(
                applicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Send(
                    expectedCommand, 
                    CancellationToken.None
                )
            );
        }
    }
}