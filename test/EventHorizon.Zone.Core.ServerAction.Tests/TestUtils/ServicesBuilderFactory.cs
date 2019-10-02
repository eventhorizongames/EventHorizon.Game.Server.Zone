using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EventHorizon.Tests.TestUtils
{
    public static class ServicesBuilderFactory
    {
        public struct ServicesBuilderMocks
        {
            public Mock<IServiceProvider> ServiceProviderMock { get; }
            public Mock<IServiceScope> ServiceScopeMock { get; }
            public Mock<IServiceScopeFactory> ServiceScopeFactoryMock { get; }
            public Mock<IServiceProvider> ApplicationServicesMock { get; }

            public ServicesBuilderMocks(
                Mock<IServiceProvider> serviceProviderMock,
                Mock<IServiceScope> serviceScopeMock,
                Mock<IServiceScopeFactory> serviceScopeFactoryMock,
                Mock<IServiceProvider> applicationServicesMock
            )
            {
                ServiceProviderMock = serviceProviderMock;
                ServiceScopeMock = serviceScopeMock;
                ServiceScopeFactoryMock = serviceScopeFactoryMock;
                ApplicationServicesMock = applicationServicesMock;
            }
        }

        public static ServicesBuilderMocks CreateServices()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );

            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );

            var applicationServicesMock = new Mock<IServiceProvider>();
            applicationServicesMock.Setup(
                mock => mock.GetService(
                    typeof(IServiceScopeFactory)
                )
            ).Returns(
                serviceScopeFactoryMock.Object
            );

            return new ServicesBuilderMocks(
                serviceProviderMock,
                serviceScopeMock,
                serviceScopeFactoryMock,
                applicationServicesMock
            );
        }
    }
}