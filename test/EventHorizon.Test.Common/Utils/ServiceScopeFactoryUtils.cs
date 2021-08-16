namespace EventHorizon.Test.Common.Utils
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using MediatR;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    [ExcludeFromCodeCoverage]
    public static class ServiceScopeFactoryUtils
    {
        public static Mock<IServiceScopeFactory> SetupServiceScopeFactoryWithMediatorMock(
            Mock<IMediator> mediatorMock
        )
        {
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceScopeFactoryMock.Setup(
                scopeFactory => scopeFactory.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );
            serviceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceProviderMock.Setup(
                serviceProvider => serviceProvider.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            return serviceScopeFactoryMock;
        }
    }
}
