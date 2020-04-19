namespace EventHorizon.Zone.System.ClientEntities.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.ClientEntities.Load;
    using EventHorizon.Zone.System.ClientEntities.State;
    using global::System;
    using global::System.Threading;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    public class SystemClientEntitiesExtensionsTests
    {
        [Fact]
        public void TestAddAgent_ShouldConfigureServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemClientEntitiesExtensions.AddSystemClientEntities(
                serviceCollectionMock
            );

            // Then
            Assert.NotEmpty(
                serviceCollectionMock
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service =>
                {
                    return typeof(ClientEntityRepository) == service.Value.ServiceType
                        && typeof(ClientEntityInMemoryRepository) == service.Value.ImplementationType;
                }
            );
        }

        [Fact]
        public void ShouldSendLoadSystemClientEntitiesCommand()
        {
            // Given
            var expected = new LoadSystemClientEntitiesCommand();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = ServiceScopeFactoryUtils.SetupServiceScopeFactoryWithMediatorMock(
                mediatorMock
            );

            var applicationServicesMock = new Mock<IServiceProvider>();
            applicationServicesMock.Setup(
                mock => mock.GetService(
                    typeof(IServiceScopeFactory)
                )
            ).Returns(
                serviceScopeFactoryMock.Object
            );

            var applicationBuilderMock = new Mock<IApplicationBuilder>();
            applicationBuilderMock.Setup(
                mock => mock.ApplicationServices
            ).Returns(
                applicationServicesMock.Object
            );

            // When
            SystemClientEntitiesExtensions.UseSystemClientEntities(
                applicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}